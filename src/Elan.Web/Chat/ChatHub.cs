using Elan.Account.Contracts;
using Elan.Chat.Contracts;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace Elan.Web.Chat
{
    public class ChatHub: Hub
    {
        private readonly IUserService _userService;
        private readonly IChatService _chatService;

        public ChatHub(IUserService userService, IChatService chatService)
        {
            _userService = userService;
            _chatService = chatService;
        }

        public async Task SendMessage(string fromUserId, string toUserId, string message)
        {
            var userFrom = await _userService.GetUser(fromUserId);
            var userTo = await _userService.GetUser(toUserId);
            
            try
            {
                var chatMessage = await _chatService.SaveMessage(userFrom, userTo, message);

                await Clients.User(toUserId).SendAsync("ReceiveMessage", JsonConvert.SerializeObject(chatMessage));
            }
            catch
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "We failed to deliver your message. Sorry :(");
            }
           
        }
    }
}
