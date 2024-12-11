using ECommerce.SharedLibrary.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.SharedLibrary.DependecyInjection
{
    public static class SharedServiceContainer
    {
        public static IServiceCollection AddSharedServices<TContext>(this IServiceCollection services, IConfiguration configuration, string fileName) where TContext: DbContext
        {
            //Add generic database context

            services.AddDbContext<TContext>(options => options.UseSqlServer(
                configuration.GetConnectionString("eCommerceConnection"), sqlServerOption =>
                sqlServerOption.EnableRetryOnFailure()));

            //Configure serilog logging
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Debug()
                .WriteTo.Console()
                .WriteTo.File(path: $"{fileName}-.text",
                restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.ff zzz} [{Level:u3}] {message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day)
                .CreateLogger();

            //Add Jwt authentication scheme
            JwtAuthenticationScheme.AddJwtAuthenticationScheme(services, configuration);

            return services;
        }

        public static IApplicationBuilder UseSharedPolicies(this IApplicationBuilder app) 
        {
            //use global exception
            app.UseMiddleware<GlobalException>();

            //Register api call
            app.UseMiddleware<ListenToOnlyApiGateway>();

            return app;
        }
    }
}
