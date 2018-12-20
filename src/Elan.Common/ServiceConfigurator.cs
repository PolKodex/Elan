using Elan.Common.Contracts;
using Elan.Common.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elan.Common
{
    public static class ServiceConfigurator
    {
        public static void RegisterCommonModule(this IServiceCollection services)
        {
            services.AddScoped<IQueryValidationService, QueryValidationService>();
        }
    }
}
