using System;

// ReSharper disable InconsistentNaming

namespace MassTransit.Saga.Demo.Contracts.Flights
{
    public interface IBookFlightRequest
    {
        Guid TripId { get; }

        DateTime DayOfFlight { get; }
        bool IsOutbound { get; }
        string From { get; }
        string To { get; }
    }
}