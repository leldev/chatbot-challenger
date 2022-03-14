using Jobsity.Chatbot.Api.Persistence.Configuration;
using Jobsity.Chatbot.Api.ServiceBus.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Jobsity.Chatbot.Api.ServiceBus.Extensions
{
    public static class ServiceBusExtension
    {
        /// <summary>
        /// Add Service Bus service for Stock Bot.
        /// </summary>
        /// <param name="services">Service Collection.</param>
        /// <param name="config">Configuration type of <see cref="IServiceBusConfiguration"/>.</param>
        /// <returns>Service Collection <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddStockService(this IServiceCollection services, IServiceBusConfiguration config)
        {
            services.AddSingleton(config);
            services.AddTransient<IStockBotService, StockBotService>();

            return services;
        }
    }
}