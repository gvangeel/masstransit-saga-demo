using System;

// ReSharper disable InconsistentNaming

namespace MassTransit.Saga.Demo.Contracts.Trips
{
    public interface ITripCancellationRequest
    {
        Guid TripId { get; }

        string Reason { get; }
    }
}
