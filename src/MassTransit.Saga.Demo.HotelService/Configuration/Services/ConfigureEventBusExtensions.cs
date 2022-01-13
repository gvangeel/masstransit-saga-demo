using MassTransit.Saga.Demo.HotelService.Application;

namespace MassTransit.Saga.Demo.HotelService.Configuration.Services;

public static class ConfigureEventBusExtensions
{
    public static void AddCustomEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddConsumersFromNamespaceContaining<TripAllFlightsBookedConsumer>();

            x.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(configuration.GetConnectionString("RabbitMq"));
                configurator.ReceiveEndpoint("hotel-service", endpointConfigurator =>
                {
                    endpointConfigurator.ConfigureConsumers(context);
                });
            });
        });

        services.AddMassTransitHostedService();
    }
}