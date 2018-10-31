using Elan.Data.Contracts;
using Elan.Data.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elan.Data
{
    public static class ServiceConfigurator
    {
        public static void RegisterDataModule(this IServiceCollection services)
        {
            services.AddScoped<IDataService, DataService>();
        }
    }
}
