using System.Collections.Generic;
using System.Threading.Tasks;
using Elan.Data.Models.Account;
using Elan.Data.Models.Posts;

namespace Elan.Posts.Contracts
{
    public interface IPostService
    {
        Task<Post> CreatePost(ElanUser createdBy, string content, ElanUser userTo = null);
        Task<List<Post>> GetLatestPostsAsync(ElanUser user, int skip = 0, int take = 10);
        Task<List<Post>> GetPostsForUserAsync(ElanUser user, ElanUser currentUser, int skip, int take);
    }
}