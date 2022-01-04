using System;
using System.Threading.Tasks;
using MassTransit.Saga.Demo.Contracts.Flights;

namespace MassTransit.Saga.Demo.TransportationService.Application
{
    public class FlightBookingConsumer : IConsumer<IBookFlightRequest>
    {
        public async Task Consume(ConsumeContext<IBookFlightRequest> context)
        {
            await Task.Delay(TimeSpan.FromSeconds(Random.Shared.Next(15, 30)));
            await context.RespondAsync<IFlightBooked>(new
            {
                context.Message.TripId,
                context.Message.IsOutbound,
                FlightId = NewId.NextGuid(),
                Cost = Convert.ToDecimal(Random.Shared.NextDouble() * 90 + 10),
                Company = Random.Shared.NextDouble() > .5d ? "Ryanair" : "EasyJet"
            });
        }
    }
}
