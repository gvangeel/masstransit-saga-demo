using System;

namespace MassTransit.Saga.Demo.Contracts.Trips
{
    public interface ITripAllFlightsBooked
    {
        Guid TripId { get; }
    }
}
