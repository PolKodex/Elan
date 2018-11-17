﻿using Elan.Account.Contracts;
using Elan.Account.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elan.Account
{
    public static class ServiceConfigurator
    {
        public static void RegisterAccountModule(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAuthValidationService, AuthValidationService>();
            services.AddScoped<IUserSettingsService, UserSettingsService>();
            services.AddScoped<IUserImageService, UserImageService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
        }
    }
}
