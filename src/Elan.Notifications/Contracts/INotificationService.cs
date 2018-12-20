using Elan.Common.Enums;
using Elan.Data.Models.Account;
using Elan.Data.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elan.Notifications.Contracts
{
    public interface INotificationService
    {
        Task<Notification> CreateNotification(string message, NotificationType notificationType, ElanUser userTo, string notificationSourceId);
        Task<List<Notification>> GetActiveNotificationsForUser(ElanUser user, int skip, int take);
        Task<int> GetNumberOfUnreadNotificationsForUser(ElanUser user);
        Task<Notification> MarkAsRead(string id);
        Task<Notification> MarkAsDeleted(string id);
        Task<bool> HasUnreadChatNotificationWithUser(ElanUser targetUser, ElanUser sourceUser);
    }
}
