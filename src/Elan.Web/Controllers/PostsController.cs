using System.Linq;
using System.Threading.Tasks;
using Elan.Posts.Contracts;
using Elan.Users.Contracts;
using Elan.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;

namespace Elan.Web.Controllers
{
    public class PostsController : ElanBaseController
    {
        private readonly IPostsService _postsService;
        private readonly IUserService _userService;

        public PostsController(IPostsService postsService, IUserService userService)
        {
            _postsService = postsService;
            _userService = userService;
        }

        [HttpPost]
        public async Task CreatePost(string content, string toUserId = null)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var userTo = await _userService.GetUserById(toUserId);

            await _postsService.CreatePost(currentUser, content, userTo);
        }

        [HttpGet]
        public async Task<JsonResult> GetLatestPosts(int skip = 0, int take = 10)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);

            var posts = await _postsService.GetLatestPostsAsync(currentUser, skip, take);

            var result = posts.Select(m => new PostViewModel(m));

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetPostsForUser(string userId, int skip = 0, int take = 10)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var user = await _userService.GetUserById(userId);

            var posts = await _postsService.GetPostsForUserAsync(user, currentUser, skip, take);

            var result = posts.Select(m => new PostViewModel(m));

            return Json(result);
        }
    }
}
