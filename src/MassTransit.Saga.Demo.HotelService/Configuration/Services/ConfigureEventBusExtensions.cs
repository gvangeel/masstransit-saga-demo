using MassTransit.Common;
using MassTransit.Saga.Demo.HotelService.Application;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.Saga.Demo.HotelService.Configuration.Services
{
    public static class ConfigureEventBusExtensions
    {
        public static void AddCustomEventBus(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumersFromNamespaceContaining<HotelBookingConsumer>();

                x.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host("localhost");
                    if (Container.IsRunningInContainer)
                    {
                        configurator.Host("rabbitmq");
                    }

                    configurator.ConfigureEndpoints(context);
                });
            });

            services.AddMassTransitHostedService();
        }
    }
}
