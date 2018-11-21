using System.Linq;
using System.Threading.Tasks;
using Elan.Common.Enums;
using Elan.Notifications.Contracts;
using Elan.Posts.Contracts;
using Elan.Posts.Models;
using Elan.Users.Contracts;
using Elan.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;

namespace Elan.Web.Controllers
{    
    // TODO: access rights need to be checked before each action executed
    // TODO: custom controller method attributes: Owner [user is owner of the entity - like in UserProfile], Participant [user is part of the entity - like in FriendsRelationRequest]
    public class PostsController : ElanBaseController
    {
        private readonly IPostsService _postsService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IPostReactionService _postReactionService;

        public PostsController(
            IPostsService postsService,
            IUserService userService,
            INotificationService notificationService,
            IPostReactionService postReactionService)
        {
            _postsService = postsService;
            _userService = userService;
            _notificationService = notificationService;
            _postReactionService = postReactionService;
        }

        [HttpPost]
        public async Task CreatePost([FromBody]CreatePostViewModel data)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var userTo = await _userService.GetUserById(data.ToUserId);

            await _postsService.CreatePost(currentUser, data.Content, null, userTo);
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

        [HttpPost]
        public async Task SetReaction([FromBody]SetPostReactionViewModel model)
        {
            model.User = await _userService.GetUserByName(HttpContext.User.Identity.Name);

            await _postReactionService.SetReaction(model);

            var post = await _postsService.GetPost(model.PostId);

            if (model.User.Id != post.CreatedBy.Id)
            {
                await _notificationService.CreateNotification(
                    $"{model.User.FirstName} {model.User.LastName} reacted to your post!", NotificationType.NewReaction,
                    post.CreatedBy);
            }

            if (model.User.Id != post.TargetUser.Id && post.TargetUser.Id != post.CreatedBy.Id)
            {
                await _notificationService.CreateNotification(
                    $"{model.User.FirstName} {model.User.LastName} reacted to a post on your wall!", NotificationType.NewReaction,
                    post.TargetUser);
            }
            
        }
    }
}
