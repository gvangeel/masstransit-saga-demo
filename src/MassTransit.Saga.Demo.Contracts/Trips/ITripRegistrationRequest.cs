// ReSharper disable InconsistentNaming

using System;

namespace MassTransit.Saga.Demo.Contracts.Trips
{
    public interface ITripRegistrationRequest
    {
        Guid TripId { get; }

        int RequiredStars { get; }
        string Destination { get; }
        DateTime Start { get; }
        DateTime End { get; }
    }
}
