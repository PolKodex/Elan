using Elan.Friends.Contracts;
using Elan.Friends.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elan.Friends
{
    public static class ServiceConfigurator
    {
        public static void RegisterFriendsModule(this IServiceCollection services)
        {
            services.AddScoped<IFriendsService, FriendsService>();
        }
    }
}
