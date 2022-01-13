using MassTransit.Saga.Demo.Contracts.Flights;
using MassTransit.Saga.Demo.Contracts.Trips;
using MassTransit.Saga.Demo.TransportationService.Domain;
using MassTransit.Saga.Demo.TransportationService.Persistence;

namespace MassTransit.Saga.Demo.TransportationService.Application;

public class TripRegisteredConsumer: IConsumer<ITripRegistered>
{
    private readonly FlightDbContext _dbContext;

    public TripRegisteredConsumer(FlightDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ITripRegistered> context)
    {
        //Outbound flight
        var flightOutBound = new FlightBooking(NewId.NextGuid(),
            true,
            Convert.ToDecimal(Random.Shared.NextDouble() * 90 + 10),
            Random.Shared.NextDouble() > .5d ? "Ryanair" : "EasyJet",
            context.Message.TripId,
            "Home",
            context.Message.Destination,
            context.Message.Start);

        await _dbContext.FlightBookings.AddAsync(flightOutBound);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        await context.Publish<IFlightBooked>(new
        {
            flightOutBound.TripId,
            IsOutbound = flightOutBound.IsOutboundFlight,
            flightOutBound.FlightId,
            Cost = flightOutBound.FlightCost,
            Company = flightOutBound.AirlineCompany
        });

        //Inbound flight
        var flightInBound = new FlightBooking(NewId.NextGuid(),
            false,
            Convert.ToDecimal(Random.Shared.NextDouble() * 90 + 10),
            Random.Shared.NextDouble() > .5d ? "Ryanair" : "EasyJet",
            context.Message.TripId,
            context.Message.Destination,
            "Home",
            context.Message.End
        );

        await _dbContext.FlightBookings.AddAsync(flightInBound);
        await _dbContext.SaveChangesAsync(CancellationToken.None);
        await context.Publish<IFlightBooked>(new
        {
            flightInBound.TripId,
            IsOutbound = flightInBound.IsOutboundFlight,
            flightInBound.FlightId,
            Cost = flightInBound.FlightCost,
            Company = flightInBound.AirlineCompany
        });
    }
}