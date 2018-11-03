using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elan.Common.Enums;
using Elan.Data.Contracts;
using Elan.Data.Models.Account;
using Elan.Data.Models.Posts;
using Elan.Posts.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Elan.Posts.Services
{
    public class PostService : IPostService
    {
        private readonly IDataService _dataService;

        public PostService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<Post> CreatePost(ElanUser createdBy, string content, ElanUser userTo = null)
        {
            var post = new Post
            {
                Content = content,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = createdBy,
                TargetUser = userTo
            };

            await _dataService.GetSet<Post>().AddAsync(post);
            await _dataService.SaveDbAsync();

            return post;
        }

        public async Task<List<Post>> GetLatestPostsAsync(ElanUser user, int skip = 0, int take = 10)
        {
            var posts =
                await _dataService
                    .GetSet<Post>()
                    .Include(m => m.CreatedBy)
                    .Include(m => m.CreatedBy.Settings)
                    .Include(m => m.TargetUser)
                    .Include(m => m.TargetUser.Settings)
                    .Where(
                        m => m.CreatedBy.Id != user.Id &&
                             m.CreatedBy.Settings
                                 .First(x => x.Setting == UserSetting.Content)
                                 .PrivacySetting == PrivacySetting.Everyone
                             || (m.TargetUser.Id == user.Id &&
                                 m.TargetUser.Settings.First(x => x.Setting == UserSetting.Content).PrivacySetting ==
                                 PrivacySetting.Everyone))
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
                    .Include(m => m.CreatedBy.Settings)
                    .Include(m => m.TargetUser)
                    .Include(m => m.TargetUser.Settings)
                    .Where(
                        m => m.CreatedBy.Id == user.Id &&
                             m.CreatedBy.Settings
                                 .First(x => x.Setting == UserSetting.Content)
                                 .PrivacySetting == PrivacySetting.Everyone
                             || (m.TargetUser.Id == user.Id &&
                                 m.TargetUser.Settings.First(x => x.Setting == UserSetting.Content).PrivacySetting ==
                                 PrivacySetting.Everyone))
                    .OrderByDescending(m => m.CreatedOn)
                    .Skip(skip)
                    .Take(take)
                    .OrderBy(m => m.CreatedOn)
                    .ToListAsync();

            return posts;
        }
    }
}
