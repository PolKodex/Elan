using Elan.Common.Enums;
using Elan.Data.Contracts;
using Elan.Data.Models.Account;
using Elan.Data.Models.Notifications;
using Elan.Notifications.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elan.Notifications.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IDataService _dataService;

        public NotificationService(IDataService dataService)
        {
            _dataService = dataService;
        }

        public async Task<Notification> CreateNotification(string message, NotificationType notificationType, ElanUser userTo, string notificationSourceId)
        {
            var notification = new Notification
            {
                IsRead = false,
                IsDeleted = false,
                Message = message,
                SentOn = DateTime.UtcNow,
                Type = notificationType,
                TargetUser = userTo,
                SourceId = notificationSourceId
            };

            await _dataService.GetSet<Notification>().AddAsync(notification);
            await _dataService.SaveDbAsync();

            return notification;
        }

        public async Task<List<Notification>> GetActiveNotificationsForUser(ElanUser user, int skip = 0, int take = 10)
        {
            var notifications = await _dataService.GetSet<Notification>()
                .Include(n => n.TargetUser)
                .Where(n => n.TargetUser.Id == user.Id && n.IsDeleted != true)
                .Skip(skip * take)
                .Take(take)
                .ToListAsync();

            return notifications;
        }

        public async Task<string> GetNumberOfUnreadNotificationsForUser(ElanUser user)
        {
            var notifications = await _dataService.GetSet<Notification>()
                .Include(n => n.TargetUser)
                .Where(n => n.TargetUser.Id == user.Id && n.IsDeleted != true && n.IsRead != true)
                .ToListAsync();

            return notifications.Count.ToString();
        }

        public async Task<Notification> MarkAsDeleted(string id)
        {
            var notification = await _dataService
                .GetSet<Notification>()
                .SingleAsync(n => n.Id.ToString() == id);

            notification.IsDeleted = true;

            await _dataService.SaveDbAsync();

            return notification;
        }

        public async Task<Notification> MarkAsRead(string id)
        {
            var notification = await _dataService
                .GetSet<Notification>()
                .SingleAsync(n => n.Id.ToString() == id);

            notification.IsRead = true;
            
            await _dataService.SaveDbAsync();

            return notification;
        }

        public async Task<bool> HasUnreadChatNotificationWithUSer(ElanUser targetUser, ElanUser sourceUser)
        {
            var notifications = await _dataService.GetSet<Notification>()
                .Include(n => n.TargetUser)
                .Where(n => n.TargetUser.Id == targetUser.Id && 
                       n.IsDeleted != true && n.IsRead != true && 
                       n.SourceId == sourceUser.Id.ToString() && 
                       n.Type == NotificationType.NewChatMessage)
                .ToListAsync();

            return notifications.Count>0;
        }
    }
}
