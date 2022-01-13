using System;
using System.Collections.Generic;
using System.Text;

namespace MassTransit.Saga.Demo.Contracts.Trips
{
    public interface ITripRegistered
    {
        Guid TripId { get; }

        int RequiredStars { get; }
        string Destination { get; }
        DateTime Start { get; }
        DateTime End { get; }
    }
}
