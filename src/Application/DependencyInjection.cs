using Application.Interfaces.Identity;
using Application.Models;
using Application.Services.Identity;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services)
        {
            // Identity Service Life Time
            services.AddScoped<ErrorDescriber>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<RoleService>()
                .AddTransient<IMapper, Mapper>();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            });

            //.AddJsonOptions(options =>
            //{
            //    options.JsonSerializerOptions.IgnoreNullValues = true;
            //})

            return services;
        }

    }
}