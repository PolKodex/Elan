using Elan.Notifications.Contracts;
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
        private readonly INotificationService _notificationService;

        private static readonly object _lock = new object();

        private static readonly Dictionary<string, string> _connections = new Dictionary<string, string>();

        public NotificationHub(IUserService userService, INotificationService notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
        }
 
        public static string GetConnectionID(string userName)
        {
            lock (_lock)
            {
                if (_connections != null && _connections.ContainsKey(userName))
                {
                    return _connections[userName];
                }
            }

            return null;
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
