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
        Task<Notification> CreateNotification(string message, NotificationType notificationType, ElanUser userTo);
        Task<List<Notification>> GetActiveNotificationsForUser(ElanUser user);
        Notification MarkAsRead(string id);
        Notification MarkAsDeleted(string id);
    }
}
