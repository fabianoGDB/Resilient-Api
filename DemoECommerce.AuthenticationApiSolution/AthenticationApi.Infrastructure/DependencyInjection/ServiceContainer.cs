using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Infrastructure.Data;
using AuthenticationApi.Infrastructure.Repositories;
using ECommerce.SharedLibrary.DependecyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration) 
        {
            //Add database connectivity
            //Add Jwt Authentication Scheme

            SharedServiceContainer.AddSharedServices<AuthenticationDbContext>(services, configuration, configuration["MySeriLog:FileName"]!);

            //Create Dependency Injection
            services.AddScoped<IUser, UserRepository>();

            return services;
        }

        public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app)
        {
            //Register Middleware
            //Global Exception
            //Listen Only to Api Gateway

            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
