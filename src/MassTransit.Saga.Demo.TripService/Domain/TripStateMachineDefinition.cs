using GreenPipes;
using MassTransit.Definition;
using MassTransit.Saga.Demo.Contracts.Flights;
using MassTransit.Saga.Demo.Contracts.Hotels;
using MassTransit.Saga.Demo.Contracts.Trips;

namespace MassTransit.Saga.Demo.TripService.Domain;

public class TripStateMachineDefinition : SagaDefinition<Trip>
{
    public TripStateMachineDefinition()
    {
        ConcurrentMessageLimit = 8;
    }

    protected override void ConfigureSaga(IReceiveEndpointConfigurator endpointConfigurator, ISagaConfigurator<Trip> sagaConfigurator)
    {
        // configure message retry (because concurrency issues throw exceptions)
        sagaConfigurator.UseMessageRetry(_ => _.Immediate(5));
        // configure the InMemoryOutbox (to avoid duplicate messages in the event of a concurrency failure)
        sagaConfigurator.UseInMemoryOutbox();

        // configure a partitioner to limit the receive endpoint to only one concurrent message for each TripId
        // (the partitioner uses hashing to meet the partition count)
        // Note: the partitioner in this case is only for this specific receive endpoint,
        // multiple service instances (competing consumers) may still consume events for the same saga instance.
        var partition = endpointConfigurator.CreatePartitioner(8);

        sagaConfigurator.Message<ITripRegistrationRequest>(x =>
            x.UsePartitioner(partition, m => m.Message.TripId));
        sagaConfigurator.Message<IFlightBooked>(x =>
            x.UsePartitioner(partition, m => m.Message.TripId));
        sagaConfigurator.Message<IHotelBooked>(x =>
            x.UsePartitioner(partition, m => m.Message.TripId));
        sagaConfigurator.Message<ITripCancellationRequest>(x =>
            x.UsePartitioner(partition, m => m.Message.TripId));
    }
}