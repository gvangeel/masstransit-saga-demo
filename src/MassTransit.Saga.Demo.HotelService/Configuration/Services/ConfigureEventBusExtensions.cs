using MassTransit.Saga.Demo.HotelService.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.Saga.Demo.HotelService.Configuration.Services
{
    public static class ConfigureEventBusExtensions
    {
        public static void AddCustomEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumersFromNamespaceContaining<HotelBookingConsumer>();

                x.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(configuration.GetConnectionString("RabbitMq"));
                    configurator.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();
        }
    }
}
