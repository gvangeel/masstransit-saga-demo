using MassTransit.EntityFrameworkCoreIntegration;
using MassTransit.Saga.Demo.Contracts.Trips;
using MassTransit.Saga.Demo.TripService.Application;
using MassTransit.Saga.Demo.TripService.Domain;
using MassTransit.Saga.Demo.TripService.Infrastructure;
using MassTransit.Saga.Demo.TripService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MassTransit.Saga.Demo.TripService.Configuration.Services;

public static class ConfigureEventBusExtensions
{
    public static void AddCustomEventBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(x =>
        {
            x.SetKebabCaseEndpointNameFormatter();

            x.AddRequestClient<ISubmitTrip>();
            x.AddRequestClient<ITripStateRequest>();
                
            x.AddSagaStateMachine<TripStateMachine, Trip>(typeof(TripStateMachineDefinition))
                .EntityFrameworkRepository(r =>
                {
                    r.UseSqlServer();
                    r.LockStatementProvider = new LockStatementProvider();
                    r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
                    r.AddDbContext<DbContext, TripDbContext>((provider, builder) =>
                    {
                        builder.UseSqlServer(configuration.GetConnectionString("trip-database"));
                    });
                    r.CustomizeQuery(trips => trips
                        .Include(_ => _.BookedFlights)
                        .Include(_ => _.HotelBooking));
                });
            
            x.AddConsumersFromNamespaceContaining<TripSubmissionConsumer>();

            x.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(configuration.GetConnectionString("RabbitMq"));
                var endpointNameFormatter = context.GetRequiredService<IEndpointNameFormatter>();
                EndpointConvention.Map<ISubmitTrip>(new Uri($"queue:{ endpointNameFormatter.Consumer<TripSubmissionConsumer>()}"));
                //EndpointConvention.Map<IBookHotelRequest>(new Uri($"queue:hotel-booking"));
                //EndpointConvention.Map<IBookFlightRequest>(new Uri($"queue:flight-booking"));

                configurator.ConfigureEndpoints(context);
                var builder = new DbContextOptionsBuilder();
                builder.UseSqlServer(configuration.GetConnectionString("trip-database"));
                configurator.UseEntityFrameworkCoreAuditStore(builder, "Audit");
            });
        });

        services.AddMassTransitHostedService();
    }
}