using Elan.Data.Models.Notifications;
using System;

namespace Elan.Web.ViewModels.Notifications
{
    public class NotificationViewModel
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public DateTime SentOn { get; set; }
        public bool IsRead { get; set; }
        public bool IsDeleted { get; set; }
        public int NotificationType { get; set; }

        public NotificationViewModel(Notification model)
        {
            Id = model.Id.ToString();
            Message = model.Message;
            SentOn = model.SentOn;
            IsRead = model.IsRead;
            IsDeleted = model.IsDeleted;
            NotificationType = (int)model.Type;
        }
    }
}
