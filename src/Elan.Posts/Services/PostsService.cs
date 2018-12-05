﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elan.Common.Enums;
using Elan.Data.Contracts;
using Elan.Data.Models.Account;
using Elan.Data.Models.Posts;
using Elan.Posts.Contracts;
using Elan.Posts.Models;
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
                    .Include(m => m.Reactions)
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
                    .Include(m => m.CreatedBy)
                    .ThenInclude(m => m.Images)
                    .Include(m => m.TargetUser)
                    .Where(m => m.BasePostId == null)
                    .Where(m =>
                        m.CreatedBy.Id == user.Id
                            ||
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
                    .Skip(skip * take)
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
                    .Include(m => m.Reactions)
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
                    .Include(m => m.CreatedBy)
                    .ThenInclude(m => m.Images)
                    .Include(m => m.TargetUser)
                    .Where(m => m.BasePostId == null)
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
                    .Skip(skip * take)
                    .Take(take)
                    .OrderBy(m => m.CreatedOn)
                    .ToListAsync();

            return posts;
        }

        public async Task DeletePost(int postId)
        {
            var post = await GetPost(postId);
            var postSet = _dataService.GetSet<Post>();

            foreach (var p in post.Comments)
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

        public Task<Post> GetPost(int postId)
        {
            return _dataService
                .GetSet<Post>()
                .Include(x => x.Comments)
                .Include(x => x.Reactions)
                .Include(x => x.TargetUser)
                .Include(x => x.CreatedBy)
                .ThenInclude(m => m.Images)
                .FirstOrDefaultAsync(x => x.Id == postId);
        }

        public async Task<List<Post>> GetPostComments(int postId, int skip = 0, int take = 10)
        {
            var result = await _dataService
                .GetSet<Post>()
                .Include(x => x.Reactions)
                .Include(x => x.CreatedBy)
                .Where(x => x.BasePostId == postId)
                .OrderByDescending(m => m.CreatedOn)
                .Skip(skip * take)
                .Take(take)
                .ToListAsync();

            return result;
        }
    }
}
