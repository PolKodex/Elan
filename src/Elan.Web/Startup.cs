using System;
using System.Text;
using System.Threading.Tasks;
using Elan.Account;
using Elan.Chat;
using Elan.Common;
using Elan.Data;
using Elan.Data.Models.Account;
using Elan.Friends;
using Elan.Posts;
using Elan.Users;
using Elan.Web.Chat;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Elan.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddDbContext<ElanDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    o => o.MigrationsAssembly("Elan.Data")));

            ConfigureIdentity(services);

            ConfigureAuthentication(services);

            services.AddCors(options => options
                .AddPolicy("CorsPolicy",
                    builder =>
                    {
                        builder.AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowAnyOrigin()
                            .AllowCredentials();
                    }));

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/build";
            });

            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
                options.HandshakeTimeout = TimeSpan.FromMilliseconds(1000);
            });

            ConfigureElanModules(services);
        }

        private static void ConfigureIdentity(IServiceCollection services)
        {
            services.AddIdentity<ElanUser, ElanRole>()
                .AddEntityFrameworkStores<ElanDbContext>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = false;
            });
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Tokens:Key"]));
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(config =>
                {
                    config.RequireHttpsMetadata = false;
                    config.SaveToken = true;
                    config.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = signingKey,
                        ValidateLifetime = true,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateIssuerSigningKey = true
                    };
                    config.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chathub"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
        }

        private static void ConfigureElanModules(IServiceCollection services)
        {
            services.RegisterAccountModule();
            services.RegisterChatModule();
            services.RegisterDataModule();
            services.RegisterFriendsModule();
            services.RegisterUsersModule();
            services.RegisterPostsModule();
            services.RegisterCommonModule();
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseExceptionHandler("/Error");
            //    app.UseHsts();
            //}

            UpdateDatabase(app);

            app.UseAuthentication();
            app.UseCors("CorsPolicy");

            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chathub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ElanDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }
    }
}
