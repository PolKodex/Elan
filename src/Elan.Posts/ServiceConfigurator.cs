using Elan.Posts.Contracts;
using Elan.Posts.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elan.Posts
{
    public static class ServiceConfigurator
    {
        public static void RegisterPostsModule(this IServiceCollection services)
        {
            services.AddScoped<IPostService, PostService>();
        }
    }
}
