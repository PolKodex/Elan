using Elan.Friends.Contracts;
using Elan.Users.Contracts;
using Elan.Web.ViewModels.Friends;
using Elan.Web.ViewModels.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Elan.Web.Controllers
{
    [Authorize]
    public class FriendsController : ElanBaseController
    {
        private readonly IFriendsService _friendsService;
        private readonly IFriendsInvitationService _friendsInvitationService;
        private readonly IUserService _userService;

        public FriendsController(IFriendsService friendsService, IUserService userService, IFriendsInvitationService friendsInvitationService)
        {
            _friendsService = friendsService;
            _userService = userService;
            _friendsInvitationService = friendsInvitationService;
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
        }

        [HttpPost]
        public async Task AcceptInvitation([FromBody]string userId)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var user = await _userService.GetUserById(userId);

            _friendsInvitationService.AcceptInvitation(user, currentUser);
            await _friendsService.CreateRelation(currentUser, user);
        }

        [HttpGet]
        public async Task<JsonResult> GetInvitationsForUser()
        {
            var user = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var invitations = await _friendsInvitationService.GetNotAcceptedFriendsInvitationsForUser(user);

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
    }
}
