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
    public static class PrivacyObjectRepository
    {
        public static IQueryable<T> HasPermissionWith<T>(
            this IQueryable<T> query, ElanUser currentUser, UserSetting setting)
            where T : PrivacyEntity
        {
            var result = query.Where(m => (m.CreatedBy.Settings
                                              .FirstOrDefault(x => x.Setting == setting)
                                              .PrivacySetting == PrivacySetting.Everyone)
            || (m.CreatedBy.Settings.First(x => x.Setting == setting).PrivacySetting == PrivacySetting.Friends && m.CreatedBy.IsFriend(currentUser))
//                                          ||
//                                          (m.CreatedBy.Settings.First(x => x.Setting == setting)
//                                               .PrivacySetting == PrivacySetting.Connections && (m.CreatedBy.IsFriend(currentUser) || m.CreatedBy.IsConnected(currentUser)))
                                          );

            return result;
        }
    }
    public class PostsService : IPostsService
    {
        private readonly IDataService _dataService;

        public PostsService(IDataService dataService)
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
                TargetUser = userTo ?? createdBy
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
                    .Include(m => m.CreatedBy.FirstUserFriends)
                    .Include(m => m.CreatedBy.SecondUserFriends)
                    .Include(m => m.TargetUser)
                    .Include(m => m.TargetUser.Settings)
                    .Where(
                        m => m.CreatedBy.Id != user.Id)
                    .HasPermissionWith(user, UserSetting.ViewPosts)
//                     &&
//                             m.CreatedBy.Settings
//                                 .First(x => x.Setting == UserSetting.ViewPosts)
//                                 .PrivacySetting == PrivacySetting.Everyone
//                             || (m.TargetUser.Id == user.Id &&
//                                 m.TargetUser.Settings.First(x => x.Setting == UserSetting.ViewPosts).PrivacySetting ==
//                                 PrivacySetting.Everyone))
                    .OrderByDescending(m => m.CreatedOn)
                    .Skip(skip)
                    .Take(take)
                    .OrderBy(m => m.CreatedOn)
                    .ToListAsync();
         //   var aaa = posts[2].CreatedBy.IsFriend(user);
          //  var xxxx= posts[2].CreatedBy.Friends.Any(x => x.FirstUserId == user.Id || x.SecondUserId == user.Id);
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
                                 .First(x => x.Setting == UserSetting.ViewPosts)
                                 .PrivacySetting == PrivacySetting.Everyone
                             || (m.TargetUser.Id == user.Id &&
                                 m.TargetUser.Settings.First(x => x.Setting == UserSetting.ViewPosts).PrivacySetting ==
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
