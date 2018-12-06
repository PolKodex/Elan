using Elan.Common.Enums;
using Elan.Data.Models.Account;
using Elan.Friends.Contracts;
using Elan.Notifications.Contracts;
using Elan.Users.Contracts;
using Elan.Web.notification;
using Elan.Web.ViewModels.Friends;
using Elan.Web.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Elan.Web.Controllers
{
    // TODO: access rights need to be checked before each action executed
    // TODO: custom controller method attributes: Owner [user is owner of the entity - like in UserProfile], Participant [user is part of the entity - like in FriendsRelationRequest]
    [Authorize]
    public class FriendsController : ElanBaseController
    {
        private readonly IFriendsService _friendsService;
        private readonly IFriendsInvitationService _friendsInvitationService;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<NotificationHub> _notificationHub;

        public FriendsController(IFriendsService friendsService, 
                                 IUserService userService, 
                                 IFriendsInvitationService friendsInvitationService,
                                 INotificationService notificationService,
                                 IHubContext<NotificationHub> notificationHub)
        {
            _friendsService = friendsService;
            _userService = userService;
            _friendsInvitationService = friendsInvitationService;
            _notificationService = notificationService;
            _notificationHub = notificationHub;
        }

        [HttpGet]
        public async Task<JsonResult> GetCurrentUserFriends()
        {
            var user = await _userService.GetUserByName(HttpContext.User.Identity.Name);

            var friends = await _friendsService.GetFriendsForUser(user);

            var result = friends.Select(f => new UserViewModel(f));

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> GetFriendsByUserId(string userId)
        {
            var user = await _userService.GetUserById(userId);

            var friends = await _friendsService.GetFriendsForUser(user);

            var result = friends.Select(f => new UserViewModel(f));

            return Json(result);
        }

        [HttpPost]
        public async Task SendInvitation([FromBody]string userId)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var user = await _userService.GetUserById(userId);

            await _friendsInvitationService.CreateInvitation(currentUser, user);

            await _notificationService.CreateNotification("User " + currentUser.GetDisplayName() + " would like to become your friend", NotificationType.FriendsInvitation, user, currentUser.Id.ToString());

            await PushNumberOfNotifications(user);
        }

        [HttpPost]
        public async Task AcceptInvitation([FromBody]string userId)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var user = await _userService.GetUserById(userId);

            await _friendsInvitationService.AcceptInvitation(currentUser, user);

            await _friendsService.CreateRelation(currentUser, user);

            await _notificationService.CreateNotification("User " + currentUser.GetDisplayName() + " has accepted your friends request", NotificationType.InvitationAccepted, user, currentUser.Id.ToString());

            await PushNumberOfNotifications(user);
        }

        [HttpPost]
        public async Task DeclineInvitation([FromBody]string userId)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var user = await _userService.GetUserById(userId);

            await _friendsInvitationService.DeclineInvitation(currentUser, user);
        }

        [HttpPost]
        public async Task CancelInvitation([FromBody]string userId)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var user = await _userService.GetUserById(userId);

            await _friendsInvitationService.CancelInvitation(currentUser, user);
        }

        [HttpPost]
        public async Task RemoveFriend([FromBody]string userId)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var user = await _userService.GetUserById(userId);

            await _friendsService.RemoveRelation(currentUser, user);
        }

        [HttpGet]
        public async Task<JsonResult> GetInvitationsForUser()
        {
            var user = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var invitations = await _friendsInvitationService.GetPendingInvitationsForUser(user);

            var result = invitations.Select(i => new FriendsInvitationViewModel(i));

            return Json(result);
        }

        [HttpGet]
        public async Task<JsonResult> HasInvitationFromUser(string userId)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var user = await _userService.GetUserById(userId);

            var result = _friendsInvitationService.IsInvitedByUser(currentUser, user);

            return Json(result.Result);
        }

        private async Task PushNumberOfNotifications(ElanUser user)
        {
            var connectionID = NotificationHub.GetConnectionID(user.UserName);
            var notificationsCount = await _notificationService.GetNumberOfUnreadNotificationsForUser(user);

            if (connectionID != null)
            {
                await _notificationHub.Clients.Client(connectionID).SendAsync("NotificationsCount", JsonConvert.SerializeObject(notificationsCount));
            }
        }
    }
}
