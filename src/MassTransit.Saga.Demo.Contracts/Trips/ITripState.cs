using System;

namespace MassTransit.Saga.Demo.Contracts.Trips
{
    public interface ITripState
    {
        Guid TripId { get; }
        DateTime Timestamp { get; }
        string State { get; }
        bool OutboundFlightBooked { get; }
        bool ReturnFlightBooked { get; }
        bool HotelBooked { get; }
    }
}
