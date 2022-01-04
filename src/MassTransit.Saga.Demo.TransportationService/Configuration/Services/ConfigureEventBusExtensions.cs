using System;
using MassTransit.Common;
using MassTransit.Saga.Demo.TransportationService.Application;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.Saga.Demo.TransportationService.Configuration.Services
{
    public static class ConfigureEventBusExtensions
    {
        public static void AddCustomEventBus(this IServiceCollection services)
        {
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumersFromNamespaceContaining<FlightBookingConsumer>();

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
