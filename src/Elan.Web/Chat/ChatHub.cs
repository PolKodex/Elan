using System;
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

        public async Task SendMessage(string toUserId, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", message);

            var userFrom = await _userService.GetUserByName(Context.User.Identity.Name);
            var userTo = await _userService.GetUserById(toUserId);
            
            try
            {
                var chatMessage = await _chatService.SaveMessage(userFrom, userTo, message);

                await Clients.User(toUserId).SendAsync("ReceiveMessage", JsonConvert.SerializeObject(chatMessage));
            }
            catch
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "We failed to deliver your message. Sorry :(");
                throw;
            }
           
        }
    }
}
