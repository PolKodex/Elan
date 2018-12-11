using Elan.Common.Enums;
using Elan.Common.Utils;
using Elan.Data.Contracts;
using Elan.Data.Models.Account;
using Elan.Data.Models.Friends;
using Elan.Data.Models.Posts;
using Elan.Posts.Contracts;
using Elan.Posts.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElanUserImage = Elan.Data.Models.Account.ElanUserImage;

namespace Elan.Posts.Services
{
    public class PostsService : IPostsService
    {
        private readonly IDataService _dataService;

        public PostsService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<Post> CreatePost(ElanUser createdBy, string content, PrivacySetting? postVisibility = null, ElanUser userTo = null)
        {
            var visibility =
                postVisibility ??
                createdBy
                    .Settings
                    .FirstOrDefault(x => x.Setting == UserSetting.ViewPosts)?
                    .PrivacySetting ??
                PrivacySetting.Friends;

            var post = new Post
            {
                Content = content,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = createdBy,
                TargetUser = userTo ?? createdBy,
                VisibilitySetting = visibility
            };

            await _dataService.GetSet<Post>().AddAsync(post);
            await _dataService.SaveDbAsync();

            return post;
        }

        public async Task<Post> CreatePostComment(ElanUser createdBy, string content, int postId)
        {
            var basePost = await GetPost(postId);

            var postComment = new Post
            {
                BasePostId = postId,
                Content = content,
                CreatedBy = createdBy,
                TargetUser = basePost.TargetUser,
                CreatedOn = DateTime.UtcNow,
                VisibilitySetting = PrivacySetting.Everyone
            };

            await _dataService.GetSet<Post>().AddAsync(postComment);
            await _dataService.SaveDbAsync();

            return postComment;
        }

        public async Task<PostListingViewModel> GetLatestPostsAsync(ElanUser user, int skip = 0, int take = 10)
        {
            var postsSet = _dataService.GetSet<Post>();

            var userFriends = await _dataService.GetSet<FriendsRelation>()
                                .Where(x => x.FirstUserId == user.Id || x.SecondUserId == user.Id)
                                .Select(x => x.FirstUserId == user.Id ? x.SecondUserId : x.FirstUserId)
                                .ToListAsync();
            userFriends.Add(user.Id);

            var userFriendsSet = new HashSet<Guid>(userFriends);

            var userPostsQuery = postsSet
                                .Where(x => x.BasePostId == null && userFriendsSet.Contains(x.CreatedById))
                                .OrderByDescending(m => m.CreatedOn);


            var totalCount = userPostsQuery.Count();
            var userPosts = await userPostsQuery.Skip(skip * take)
                               .Take(take)
                               .Include(m => m.CreatedBy)
                               .Include(m => m.Reactions)
                               .ToListAsync();

            var filteredUsers = userFriendsSet.Where(x => userPosts.Any(y => y.CreatedById == x));
            var filteredUsersImages = await GetUserAvatarThumbnails(filteredUsers);

            foreach (var post in userPosts)
            {
                post.CommentsCount = postsSet.Count(x => x.BasePostId == post.Id);
                post.UserImage = filteredUsersImages[post.CreatedById];
            }

            var model = new PostListingViewModel
            {
                Posts = userPosts,
                TotalCount = totalCount
            };

            return model;
        }

        public async Task<PostListingViewModel> GetPostsForUserAsync(ElanUser user, ElanUser currentUser, int skip, int take)
        {
            var postsSet = _dataService.GetSet<Post>();

            var userPostsQuery = postsSet
                .Where(x => x.BasePostId == null && x.CreatedById == user.Id);

            if (user.Friends.All(x => x.FirstUserId != currentUser.Id && x.SecondUserId != currentUser.Id))
            {
                userPostsQuery = postsSet
                    .Where(x => x.VisibilitySetting == PrivacySetting.Everyone);
            }

            var totalCount = userPostsQuery.Count();
            var posts = await userPostsQuery.OrderByDescending(m => m.CreatedOn)
                            .Skip(skip * take)
                            .Take(take)
                            .Include(m => m.CreatedBy)
                            .Include(m => m.Reactions)
                            .ToListAsync();

            var users = posts.Select(x => x.CreatedById).Distinct();
            var authorsImages = await GetUserAvatarThumbnails(users);

            foreach (var post in posts)
            {
                post.CommentsCount = postsSet.Count(x => x.BasePostId == post.Id);
                post.UserImage = authorsImages[post.CreatedById];
            }

            var model = new PostListingViewModel
            {
                Posts = posts,
                TotalCount = totalCount
            };

            return model;
        }

        public async Task DeletePost(int postId)
        {
            var post = await GetPost(postId);
            var postSet = _dataService.GetSet<Post>();

            var postComments = postSet.Where(x => x.BasePostId == postId);

            foreach (var p in postComments)
            {
                postSet.Remove(p);
            }

            postSet.Remove(post);

            await _dataService.SaveDbAsync();
        }

        public async Task EditPost(PostViewModel data)
        {
            if (data.PostId == null)
            {
                return;
            }

            var post = await GetPost(data.PostId.Value);

            post.Content = data.Content;

            if (post.BasePostId == null && data.PrivacySetting != null)
            {
                post.VisibilitySetting = data.PrivacySetting.Value;
            }

            post.ModifiedOn = DateTime.UtcNow;

            await _dataService.SaveDbAsync();
        }

        public async Task<Post> GetPost(int postId)
        {
            var postsSet = _dataService.GetSet<Post>();

            var post = await postsSet
                .Include(x => x.Reactions)
                .Include(x => x.TargetUser)
                .Include(x => x.CreatedBy)
                .ThenInclude(m => m.Images)
                .FirstOrDefaultAsync(x => x.Id == postId);

            post.CommentsCount = postsSet.Count(x => x.BasePostId == postId);

            return post;
        }

        public async Task<PostListingViewModel> GetPostComments(int postId, int skip = 0, int take = 10)
        {
            var commentsQuery = _dataService
                .GetSet<Post>()
                .Where(x => x.BasePostId == postId);

            var totalCount = commentsQuery.Count();
            var comments = await commentsQuery
                .OrderByDescending(m => m.CreatedOn)
                .Skip(skip * take)
                .Take(take)
                .Include(x => x.Reactions)
                .Include(x => x.CreatedBy)
                .ToListAsync();

            var users = comments.Select(x => x.CreatedById).Distinct();
            var authorsImages = await GetUserAvatarThumbnails(users);

            foreach (var post in comments)
            {
                post.UserImage = authorsImages[post.CreatedById];
            }

            var model = new PostListingViewModel
            {
                Posts = comments,
                TotalCount = totalCount
            };

            return model;
        }
        private async Task<Dictionary<Guid, ElanUserImage>> GetUserAvatarThumbnails(IEnumerable<Guid> users)
        {
            var filteredUsersImages = new Dictionary<Guid, ElanUserImage>();
            foreach (var friend in users)
            {
                var userMainImage = await _dataService.GetSet<ElanUserImage>()
                    .FirstOrDefaultAsync(x => x.UserId == friend && x.IsMain);

                if (userMainImage != null)
                {
                    userMainImage.RawValue = ImageUtil.Resize(userMainImage.RawValue, 30, 30);
                }

                filteredUsersImages.Add(friend, userMainImage);
            }

            return filteredUsersImages;
        }
    }
}
