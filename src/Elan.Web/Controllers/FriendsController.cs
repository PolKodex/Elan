using Elan.Friends.Contracts;
using Elan.Users.Contracts;
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
        private readonly IUserService _userService;

        public FriendsController(IFriendsService friendsService, IUserService userService)
        {
            _friendsService = friendsService;
            _userService = userService;
        }

        [HttpPost]
        public async Task CreateFriendsRelation(string userToId, string secondUserId)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var userTo = await _userService.GetUserById(userToId);

            await _friendsService.CreateRelation(currentUser, userTo);
        }

        [HttpGet]
        public async Task<JsonResult> GetFriends(string userId)
        {
            var user = await _userService.GetUserById(userId);

            var friends = await _friendsService.GetFriendsForUser(user);

            var result = friends.Select(f => new UserViewModel(f));

            return Json(result);
        }
    }
}
