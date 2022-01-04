using System;

// ReSharper disable InconsistentNaming

namespace MassTransit.Saga.Demo.Contracts.Trips
{
    public interface ITripStateRequest
    {
        Guid TripId { get; }
    }
}
