using System;

namespace MassTransit.Saga.Demo.Contracts.Trips
{
    public interface ISubmitTrip
    {
        Guid TripId { get; }

        int RequiredStars { get; }
        string Destination { get; }
        DateTime Start { get; }
        DateTime End { get; }
    }
}
