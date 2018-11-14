using Elan.Users.Contracts;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elan.Web.notification
{
    public class NotificationHub : Hub
    {
        private readonly IUserService _userService;

        private readonly object _lock = new object();

        private static readonly Dictionary<string, string> _connections = new Dictionary<string, string>();

        public NotificationHub(IUserService userService)
        {
            _userService = userService;
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
