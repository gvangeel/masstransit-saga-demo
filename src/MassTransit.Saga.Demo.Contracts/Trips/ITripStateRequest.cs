using System;

namespace MassTransit.Saga.Demo.Contracts.Trips
{
    public interface ITripStateRequest
    {
        Guid TripId { get; }
    }
}
