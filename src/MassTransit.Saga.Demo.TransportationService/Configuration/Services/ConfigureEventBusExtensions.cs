﻿using MassTransit.Saga.Demo.TransportationService.Application;

namespace MassTransit.Saga.Demo.TransportationService.Configuration.Services;

public static class ConfigureEventBusExtensions
{
    public static void AddCustomEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumersFromNamespaceContaining<FlightBookingConsumer>();

            x.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(configuration.GetConnectionString("RabbitMq"));
                configurator.ConfigureEndpoints(context);
            });
        });
    }
}