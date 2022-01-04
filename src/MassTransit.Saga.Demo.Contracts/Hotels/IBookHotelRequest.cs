﻿using System;

// ReSharper disable InconsistentNaming

namespace MassTransit.Saga.Demo.Contracts.Hotels
{
    public interface IBookHotelRequest
    {
        Guid TripId { get; }

        int RequiredStars { get; }
        string Location { get; }
    }
}