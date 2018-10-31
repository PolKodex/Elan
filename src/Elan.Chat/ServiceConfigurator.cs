using Elan.Chat.Contracts;
using Elan.Chat.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elan.Chat
{
    public static class ServiceConfigurator
    {
        public static void RegisterChatModule(this IServiceCollection services)
        {
            services.AddScoped<IChatService, ChatService>();
        }
    }
}
