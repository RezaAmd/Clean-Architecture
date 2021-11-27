using Application.Interfaces;
using Application.Interfaces.Context;
using Application.Models;
using Domain.Entities.Identity;
using Infrastructure.Common.Interfaces;
using Infrastructure.Common.Models;
using Infrastructure.Common.Services;
using Infrastructure.Persistence.Configs;
using Infrastructure.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            #region Introduction
            services.AddDbContext<IdentityDbContext>(options =>
            options.UseSqlServer(ConnectionStrings.Identity,
            b => b.MigrationsAssembly(typeof(IdentityDbContext).Assembly.FullName)));
            #endregion

            #region Options
            // Identity Options when user register or login.
            services.Configure<IdentityOptions>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                // User settings.
                options.User.AllowedUserNameCharacters = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ_.";
                options.User.RequireUniqueEmail = false;
            });
            #endregion

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<IdentityDbContext>()
                .AddErrorDescriber<ErrorDescriber>()
                .AddDefaultTokenProviders();

            services.AddScoped<IDbContext, IdentityDbContext>()
                .AddRestServices();

            return services;
        }

        public static IServiceCollection AddNotifications(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddTransient<ISmsService, SmsService>()
                .AddTransient<IEmailService, EmailService>();

            services.Configure<EmailSettings>(Configuration.GetSection("EmailConfiguration"));
            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
        {
            services.AddSingleton<IJwtService, JwtService>();

            services.AddAuthentication(cfg =>
            {
                cfg.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                cfg.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    var key = Encoding.UTF8.GetBytes(JwtConfig.secretKey);
                    var encryptionkey = Encoding.UTF8.GetBytes(JwtConfig.encryptionKey);
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ClockSkew = TimeSpan.Zero,
                        RequireSignedTokens = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        TokenDecryptionKey = new SymmetricSecurityKey(encryptionkey)
                    };
                });

            return services;
        }

        public static IServiceCollection AddCacheServices(this IServiceCollection services)
        {
            services.AddTransient<IRedisService, RedisService>();

            services.AddDistributedMemoryCache();
            // Redis Cash Service
            services.AddStackExchangeRedisCache(option =>
            {
                option.Configuration = "localhost:REDIS_PORT,password=REDIS_PASSWORD";
            });
            return services;
        }
    }
}