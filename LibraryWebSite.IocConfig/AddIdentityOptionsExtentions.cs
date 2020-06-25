using System;
using System.Collections.Generic;
using System.Text;
using LibraryWebSite.Data;
using LibraryWebSite.Entities.Identity;
using LibraryWebSite.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryWebSite.IocConfig
{
    public static class AddIdentityOptionsExtentions
    {
        public static IServiceCollection AddIdentityOptions(this IServiceCollection services)
        {
            services.AddIdentity<User, Role>(
                options =>
                {
                    //Configure Password
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequiredUniqueChars = 1;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;

                    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                    options.User.RequireUniqueEmail = true;

                    options.SignIn.RequireConfirmedEmail = true;

                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
                    options.Lockout.MaxFailedAccessAttempts = 3;
                })
             .AddEntityFrameworkStores<LibraryDBContext>()
             .AddErrorDescriber<CustomIdentityErrorDescriber>()
             .AddDefaultTokenProviders();


            return services;
        }
    }
}
