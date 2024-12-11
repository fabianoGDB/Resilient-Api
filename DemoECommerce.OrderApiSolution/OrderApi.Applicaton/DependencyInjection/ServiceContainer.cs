using ECommerce.SharedLibrary.Logs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Applicaton.Services;
using Polly;
using Polly.Retry;

namespace OrderApi.Applicaton.DependencyInjection
{
    public static class ServiceContainer
    {

        public static IServiceCollection AddApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            //Register HttpClient service
            //Crete Dependency Injection
            services.AddHttpClient<IOrderServices, OrderService>(options =>
            {
                options.BaseAddress = new Uri(configuration["ApiGateway:BaseAddress"]!);
                options.Timeout = TimeSpan.FromSeconds(1);
            });

            //Create Retry Strategy
            var retryStrategy = new RetryStrategyOptions()
            {
                ShouldHandle = new PredicateBuilder().Handle<TaskCanceledException>(),
                BackoffType = DelayBackoffType.Constant,
                UseJitter = true,
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromMilliseconds(500),
                OnRetry = args =>
                {
                    string message = $"OnRetry, Attemps: {args.AttemptNumber} Outcome {args.Outcome}";
                    LogException.LogToConsole(message);
                    LogException.LogToDebugger(message);
                    return ValueTask.CompletedTask;
                }
            };

            //Use Retry Strategy

            services.AddResiliencePipeline("my-retry-pipeline", builder =>
            {
                builder.AddRetry(retryStrategy);
            });

            return services;
        }
    }
}
