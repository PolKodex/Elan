using Elan.Chat.Contracts;
using Elan.Users.Contracts;
using Elan.Web.ViewModels.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Elan.Web.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class ChatController: Controller
    {
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        public ChatController(IChatService chatService, IUserService userService)
        {
            _chatService = chatService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<JsonResult> GetMessages(string userId, int skip = 0, int take = 10)
        {
            var user = await _userService.GetUserById(userId);
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);

            var messages = await _chatService.GetMessagesAsync(user, currentUser, skip, take);

            var result = messages.Select(m => new ChatMessageViewModel(m, currentUser.Id.ToString()));

            return Json(result);
        }
    }
}
