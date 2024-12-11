using ECommerce.SharedLibrary.DependecyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Applicaton.Interfaces;
using OrderApi.Applicaton.Services;
using OrderApi.Infrasructure.Data;
using OrderApi.Infrasructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApi.Infrasructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructurService(this IServiceCollection services, IConfiguration configuration) 
        {
            SharedServiceContainer.AddSharedServices<OrderDbContext>(services, configuration, configuration["MySerilog:FileName"]!);

            services.AddScoped<IOrder, OrderRepository>();

            return services;
        }

        public static IApplicationBuilder UserInfrastructurePolicy(this IApplicationBuilder app) 
        {
            //Register middleware exception
            //Global exception -> handle external errors
            //ListenToApiGateway Only
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
