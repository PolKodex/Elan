using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elan.Common.Enums;
using Elan.Data.Contracts;
using Elan.Data.Models.Account;
using Elan.Data.Models.Posts;
using Elan.Posts.Contracts;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Post>> GetLatestPostsAsync(ElanUser user, int skip = 0, int take = 10)
        {
            var posts =
                await _dataService
                    .GetSet<Post>()
                    .Include(m => m.CreatedBy)
                    .ThenInclude(m => m.FirstUserFriends)
                    .ThenInclude(m => m.SecondUser.FirstUserFriends)
                    .Include(m => m.CreatedBy)
                    .ThenInclude(m => m.FirstUserFriends)
                    .ThenInclude(m => m.SecondUser.SecondUserFriends)
                    .Include(m => m.CreatedBy)
                    .ThenInclude(m => m.SecondUserFriends)
                    .ThenInclude(m => m.FirstUser.FirstUserFriends)
                    .Include(m => m.CreatedBy)
                    .ThenInclude(m => m.SecondUserFriends)
                    .ThenInclude(m => m.FirstUser.SecondUserFriends)
                    .Include(m => m.TargetUser)
                    .Where(m => !m.BasePostId.HasValue)
                    .Where(m => m.CreatedBy.Id != user.Id)
                    .Where(m =>
                        (
                            (m.VisibilitySetting == PrivacySetting.Friends ||
                             m.VisibilitySetting == PrivacySetting.Connections)
                            && (m.CreatedBy.FirstUserFriends.Any(x => x.SecondUserId == user.Id)
                                ||
                                m.CreatedBy.SecondUserFriends.Any(x => x.FirstUserId == user.Id))
                        )
                        ||
                        (
                            m.VisibilitySetting == PrivacySetting.Connections
                            && (m.CreatedBy.FirstUserFriends.Any(x =>
                                    x.SecondUser.FirstUserFriends.Any(y => y.SecondUserId == user.Id) ||
                                    x.SecondUser.SecondUserFriends.Any(y => y.FirstUserId == user.Id))
                                ||
                                m.CreatedBy.SecondUserFriends.Any(x =>
                                    x.FirstUser.FirstUserFriends.Any(y => y.SecondUserId == user.Id) ||
                                    x.FirstUser.SecondUserFriends.Any(y => y.FirstUserId == user.Id)))
                        )
                    )
                    .OrderByDescending(m => m.CreatedOn)
                    .Skip(skip)
                    .Take(take)
                    .OrderBy(m => m.CreatedOn)
                    .ToListAsync();

            return posts;
        }

        public async Task<List<Post>> GetPostsForUserAsync(ElanUser user, ElanUser currentUser, int skip, int take)
        {
            var posts =
                await _dataService
                    .GetSet<Post>()
                    .Include(m => m.CreatedBy)
                    .ThenInclude(m => m.FirstUserFriends)
                    .ThenInclude(m => m.SecondUser.FirstUserFriends)
                    .Include(m => m.CreatedBy)
                    .ThenInclude(m => m.FirstUserFriends)
                    .ThenInclude(m => m.SecondUser.SecondUserFriends)
                    .Include(m => m.CreatedBy)
                    .ThenInclude(m => m.SecondUserFriends)
                    .ThenInclude(m => m.FirstUser.FirstUserFriends)
                    .Include(m => m.CreatedBy)
                    .ThenInclude(m => m.SecondUserFriends)
                    .ThenInclude(m => m.FirstUser.SecondUserFriends)
                    .Include(m => m.TargetUser)
                    .Where(m => !m.BasePostId.HasValue)
                    .Where(m => m.CreatedBy.Id == user.Id || m.TargetUser.Id == user.Id)
                    .Where(m =>
                        m.VisibilitySetting == PrivacySetting.Everyone
                        ||
                        (
                            (m.VisibilitySetting == PrivacySetting.Friends ||
                             m.VisibilitySetting == PrivacySetting.Connections)
                            && (m.CreatedBy.FirstUserFriends.Any(x => x.SecondUserId == currentUser.Id)
                                ||
                                m.CreatedBy.SecondUserFriends.Any(x => x.FirstUserId == currentUser.Id))
                        )
                        ||
                        (
                            m.VisibilitySetting == PrivacySetting.Connections
                            && (m.CreatedBy.FirstUserFriends.Any(x =>
                                    x.SecondUser.FirstUserFriends.Any(y => y.SecondUserId == currentUser.Id) ||
                                    x.SecondUser.SecondUserFriends.Any(y => y.FirstUserId == currentUser.Id))
                                ||
                                m.CreatedBy.SecondUserFriends.Any(x =>
                                    x.FirstUser.FirstUserFriends.Any(y => y.SecondUserId == currentUser.Id) ||
                                    x.FirstUser.SecondUserFriends.Any(y => y.FirstUserId == currentUser.Id)))
                        )
                    )
                    .OrderByDescending(m => m.CreatedOn)
                    .Skip(skip)
                    .Take(take)
                    .OrderBy(m => m.CreatedOn)
                    .ToListAsync();

            return posts;
        }

        public Task<Post> GetPost(int postId)
        {
            return _dataService
                .GetSet<Post>()
                .Include(x => x.Reactions)
                .Include(x => x.TargetUser)
                .Include(x => x.CreatedBy)
                .FirstOrDefaultAsync(x => x.Id == postId);
        }
    }
}
