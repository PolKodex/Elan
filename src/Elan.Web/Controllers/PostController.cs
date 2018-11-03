using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elan.Data.Models.Posts;
using Elan.Posts.Contracts;
using Elan.Users.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Elan.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly IUserService _userService;

        public PostController(IPostService postService, IUserService userService)
        {
            _postService = postService;
            _userService = userService;
        }

        [HttpPost]
        public async Task CreatePost(string content, string toUserId = null)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var userTo = await _userService.GetUserById(toUserId);

            await _postService.CreatePost(currentUser,content,userTo);
        }

        //        [HttpGet]
        //        public async Task<List<Post>> GetLatestsPosts(int skip = 0, int take = 10)
        //        {
        //            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
        //
        //            var posts = await _postService.GetLatestPostsAsync(currentUser, skip, take);
        //            return posts;
        //        }
        //
        //        [HttpGet]
        //        public async Task<List<Post>> GetPostsForUser(string userId, int skip = 0, int take = 10)
        //        {
        //            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
        //            var user = await _userService.GetUserById(userId);
        //
        //            var posts = await _postService.GetPostsForUserAsync(user, currentUser, skip, take);
        //            return posts;
        //        }

        [HttpGet]
        public async Task<JsonResult> GetLatestsPosts(int skip = 0, int take = 10)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);

            var posts = await _postService.GetLatestPostsAsync(currentUser, skip, take);
            return Json(posts);
        }

        [HttpGet]
        public async Task<List<Post>> GetPostsForUser(string userId, int skip = 0, int take = 10)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var user = await _userService.GetUserById(userId);

            var posts = await _postService.GetPostsForUserAsync(user, currentUser, skip, take);
            return posts;
        }

    }
}