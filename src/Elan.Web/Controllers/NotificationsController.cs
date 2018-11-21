using Elan.Notifications.Contracts;
using Elan.Users.Contracts;
using Elan.Web.ViewModels.Notifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elan.Web.Controllers
{
    [Authorize]
    public class NotificationsController: ElanBaseController
    {
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public NotificationsController(INotificationService notificationService, IUserService userService)
        {
            _notificationService = notificationService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<JsonResult> GetNotifications()
        {
            var currentUser = await _userService.GetUserByName(HttpContext.User.Identity.Name);

            var notifications = await _notificationService.GetActiveNotificationsForUser(currentUser);

            var result = notifications.Select(n => new NotificationViewModel(n));

            return Json(result);
        }

        [HttpPost]
        public async Task MarkAsRead([FromBody]string notificationId)
        {
            await _notificationService.MarkAsRead(notificationId);
        }

        [HttpPost]
        public async Task MarkAsDeleted([FromBody]string notificationId)
        {
           await _notificationService.MarkAsDeleted(notificationId);
        }
    }
}
