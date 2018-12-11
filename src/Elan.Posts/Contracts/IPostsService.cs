using System.Collections.Generic;
using System.Threading.Tasks;
using Elan.Common.Enums;
using Elan.Data.Models.Account;
using Elan.Data.Models.Posts;
using Elan.Posts.Models;

namespace Elan.Posts.Contracts
{
    public interface IPostsService
    {
        Task<Post> CreatePost(ElanUser createdBy, string content, PrivacySetting? postVisibility = null, ElanUser userTo = null);
        Task<Post> CreatePostComment(ElanUser createdBy, string content, int postId);
        Task<PostListingViewModel> GetLatestPostsAsync(ElanUser user, int skip = 0, int take = 10);
        Task<PostListingViewModel> GetPostsForUserAsync(ElanUser user, ElanUser currentUser, int skip, int take);
        Task DeletePost(int postId);
        Task EditPost(PostViewModel data);
        Task<Post> GetPost(int postId);
        Task<PostListingViewModel> GetPostComments(int postId, int skip = 0, int take = 10);
    }
}
