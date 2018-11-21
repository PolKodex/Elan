using Elan.Notifications.Contracts;
using Elan.Notifications.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elan.Notifications
{
    public static class ServiceConfigurator
    {
        public static void RegisterNotificationsModule(this IServiceCollection services)
        {
            services.AddScoped<INotificationService, NotificationService>();
        }
    }
}