using System;

namespace MassTransit.Saga.Demo.Contracts.Flights
{
    public interface IFlightBooked
    {
        Guid TripId { get; }
        bool IsOutbound { get; }
        Guid FlightId { get; }
        decimal Cost { get; }
        string Company { get; }
    }
}