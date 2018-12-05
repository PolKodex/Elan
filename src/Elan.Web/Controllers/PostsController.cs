﻿using Elan.Common.Enums;
using Elan.Notifications.Contracts;
using Elan.Posts.Contracts;
using Elan.Posts.Models;
using Elan.Users.Contracts;
using Microsoft.AspNetCore.Authorization;
using Elan.Web.notification;
using Elan.Web.ViewModels.Posts;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Elan.Data.Models.Account;
using Elan.Data.Models.Posts;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Elan.Account.Contracts;

namespace Elan.Web.Controllers
{
    // TODO: access rights need to be checked before each action executed
    // TODO: custom controller method attributes: Owner [user is owner of the entity - like in UserProfile], Participant [user is part of the entity - like in FriendsRelationRequest]
    [Authorize]
    public class PostsController : ElanBaseController
    {
        private readonly IPostsService _postsService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IPostReactionService _postReactionService;
        private readonly IHubContext<NotificationHub> _notificationHub;

        public PostsController(
            IPostsService postsService,
            IUserService userService,
            INotificationService notificationService,
            IPostReactionService postReactionService,
            IHubContext<NotificationHub> notificationHub)
        {
            _postsService = postsService;
            _userService = userService;
            _notificationService = notificationService;
            _postReactionService = postReactionService;
            _notificationHub = notificationHub;
        }

        [HttpPost]
        public async Task CreatePost([FromBody]Posts.Models.PostViewModel data)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var userTo = await _userService.GetUserById(data.ToUserId);

            await _postsService.CreatePost(currentUser, data.Content, null, userTo);
        }

        [HttpPost]
        public async Task CreatePostComment([FromBody]Posts.Models.PostViewModel data)
        {
            if (data.BasePostId == null)
            {
                return;
            }

            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);

            await _postsService.CreatePostComment(currentUser, data.Content, data.BasePostId.Value);
        }

        [HttpGet]
        public async Task<JsonResult> GetLatestPosts(int skip = 0, int take = 10)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);

            var posts = await _postsService.GetLatestPostsAsync(currentUser, skip, take);

            var result = posts.Select(m => new ViewModels.Posts.PostViewModel(m));

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetPostsForUser(string userId, int skip = 0, int take = 10)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var user = await _userService.GetUserById(userId);

            var posts = await _postsService.GetPostsForUserAsync(user, currentUser, skip, take);

            var result = posts.Select(m => new ViewModels.Posts.PostViewModel(m));

            return Json(result);
        }

        [HttpPost]
        public async Task SetReaction([FromBody]SetPostReactionViewModel model)
        {
            model.User = await _userService.GetUserByName(HttpContext.User.Identity.Name);

            await _postReactionService.SetReaction(model);

            var post = await _postsService.GetPost(model.PostId);

            if (post.Reactions.FirstOrDefault(x => x.UserId == model.User.Id) == null)
            {
                return;
            }

            await PushNumberOfPostReactions(model);

            if (model.User.Id != post.CreatedBy.Id)
            {
                await _notificationService.CreateNotification(
                    $"{model.User.FirstName} {model.User.LastName} reacted to your post!", NotificationType.NewReaction,
                    post.CreatedBy);

                await PushNumberOfNotifications(post.CreatedBy);
            }

            if (model.User.Id != post.TargetUser.Id && post.TargetUser.Id != post.CreatedBy.Id)
            {
                await _notificationService.CreateNotification(
                    $"{model.User.FirstName} {model.User.LastName} reacted to a post on your wall!",
                    NotificationType.NewReaction,
                    post.TargetUser);

                await PushNumberOfNotifications(post.TargetUser);
            }
        }

        [HttpDelete]
        public async Task DeletePost(int postId)
        {
            await _postsService.DeletePost(postId);
        }

        [HttpPut]
        public async Task EditPost([FromBody]Posts.Models.PostViewModel data)
        {
            await _postsService.EditPost(data);
        }

        [HttpGet]
        public async Task<JsonResult> GetPostComments(int postId, int skip = 0, int take = 10)
        {
            var comments = await _postsService.GetPostComments(postId, skip, take);

            var result = comments.Select(m => new ViewModels.Posts.PostViewModel(m));

            return Json(result);
        }

        private async Task PushNumberOfNotifications(ElanUser user)
        {
            var connectionID = NotificationHub.GetConnectionID(user.UserName);
            var notificationsCount = _notificationService.GetNumberOfUnreadNotificationsForUser(user);

            if (connectionID != null)
            {
                await _notificationHub.Clients.Client(connectionID).SendAsync("NotificationsCount", JsonConvert.SerializeObject(notificationsCount));
            }
        }

        private async Task PushNumberOfPostReactions(SetPostReactionViewModel postReaction)
        {
            var connectionID = NotificationHub.GetConnectionID(postReaction.User.UserName);
            var reactionsCount = _postReactionService.GetReactionCount(postReaction.PostId);

            if (connectionID != null)
            {
                await _notificationHub.Clients.All.SendAsync("ReactionsCount", JsonConvert.SerializeObject(reactionsCount));
            }
        }
    }
}
