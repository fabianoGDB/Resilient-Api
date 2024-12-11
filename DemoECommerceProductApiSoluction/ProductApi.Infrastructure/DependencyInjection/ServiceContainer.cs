using ECommerce.SharedLibrary.DependecyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductApi.Application.Interfaces;
using ProductApi.Infrastructure.Data;
using ProductApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer { 
    
        public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration configuration)
        {
            //Add database conection
            //Add authentication scheme
            SharedServiceContainer.AddSharedServices<ProductDbContext>(services, configuration, configuration["MySerilog:FileName"]!);


            //create dependency injection
            services.AddScoped<IProduct, ProductRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app) 
        {
            //Register middleware as:
            //GloblalException
            //Listen to only api gateway
            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
