using Elan.Common.Enums;
using Elan.Data.Models.Account;
using Elan.Notifications.Contracts;
using Elan.Users.Contracts;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elan.Web.notification
{
    public class NotificationHub : Hub
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        private readonly object _lock = new object();

        private static readonly Dictionary<string, string> _connections = new Dictionary<string, string>();

        public NotificationHub(IUserService userService, INotificationService notificationService)
        {
            _userService = userService;
            _notificationService = notificationService;
        }
 
        public async void GetNumberOfNotifications()
        {
            string userName = Context.User.Identity.Name;
            ElanUser user = await _userService.GetUserByName(userName);
            try
            {
                string notificationsCount = await _notificationService.GetNumberOfUnreadNotificationsForUser(user);

                if (_connections.ContainsKey(userName))
                {
                    await Clients.Client(_connections[userName]).SendAsync("NotificationsCount", JsonConvert.SerializeObject(notificationsCount));
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        public async void SendNotification(ElanUser sentTo, NotificationType type, string notificationContent)
        {
            await _notificationService.CreateNotification(notificationContent, type, sentTo);
            try
            {
                string notificationsCount = await _notificationService.GetNumberOfUnreadNotificationsForUser(sentTo);

                if (_connections.ContainsKey(sentTo.UserName))
                {
                    await Clients.Client(_connections[sentTo.UserName]).SendAsync("NotificationsCount", JsonConvert.SerializeObject(notificationsCount));
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
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
