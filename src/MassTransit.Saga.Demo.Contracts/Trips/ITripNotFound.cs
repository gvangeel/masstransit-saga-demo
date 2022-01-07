using System;

namespace MassTransit.Saga.Demo.Contracts.Trips
{
    public interface ITripNotFound
    {
        Guid TripId { get; }
    }
}
