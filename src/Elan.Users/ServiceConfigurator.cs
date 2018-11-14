using Elan.Users.Contracts;
using Elan.Users.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elan.Users
{
    public static class ServiceConfigurator
    {
        public static void RegisterUsersModule(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserSearchService, UserSearchService>();
        }
    }
}
