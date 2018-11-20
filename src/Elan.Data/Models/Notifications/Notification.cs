using Elan.Common.Enums;
using Elan.Data.Models.Account;
using System;

namespace Elan.Data.Models.Notifications
{
    public class Notification
    {
        public Guid Id { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }
        public string Message { get; set; }
        public DateTime SentOn { get; set; }
        public NotificationType Type { get; set; }
        public virtual ElanUser TargetUser { get; set; }
    }
}
