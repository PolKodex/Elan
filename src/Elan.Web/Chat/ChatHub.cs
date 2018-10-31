using Elan.Chat.Contracts;
using Elan.Users.Contracts;
using Elan.Web.ViewModels.Chat;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elan.Web.Chat
{
    public class ChatHub: Hub
    {
        private readonly IUserService _userService;
        private readonly IChatService _chatService;

        private readonly object _lock = new object();

        private static readonly Dictionary<string, string> _connections = new Dictionary<string, string>();

        public ChatHub(IUserService userService, IChatService chatService)
        {
            _userService = userService;
            _chatService = chatService;
        }

        public async Task SendMessage(string toUserId, string message)
        {
            var userFrom = await _userService.GetUserByName(Context.User.Identity.Name);
            var userTo = await _userService.GetUserById(toUserId);
            
            try
            {
                var chatMessage = await _chatService.SaveMessage(userFrom, userTo, message);

                var chatMessageViewModel = new ChatMessageViewModel(chatMessage);

                if (_connections.ContainsKey(userTo.UserName))
                {
                    await Clients.Client(_connections[userTo.UserName]).SendAsync("ReceiveMessage", JsonConvert.SerializeObject(chatMessageViewModel));

                }

                await Clients.Caller.SendAsync("ReceiveMessage", JsonConvert.SerializeObject(chatMessageViewModel));
            }
            catch
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "We failed to deliver your message. Sorry :(");
                throw;
            }   
        }
        public override Task OnConnectedAsync()
        {
            var userName = Context.User.Identity.Name;

            lock (_lock)
            {
                if (!_connections.ContainsKey(userName))
                {
                    _connections.Add(userName, Context.ConnectionId);
                }
            }
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            var userName = Context.User.Identity.Name;

            lock (_lock)
            {
                if (_connections.ContainsKey(userName))
                {
                    _connections.Remove(userName);
                }
            }

            return base.OnDisconnectedAsync(exception);
        }
    }
}
