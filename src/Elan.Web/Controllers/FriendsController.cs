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
        public async Task CreateFriendsRelation([FromBody]string userId)
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);
            var user = await _userService.GetUserById(userId);

            await _friendsService.CreateRelation(currentUser, user);
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
    }
}
