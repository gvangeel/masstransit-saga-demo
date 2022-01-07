using System;

namespace MassTransit.Saga.Demo.Contracts.Hotels
{
    public interface IHotelBooked
    {
        Guid TripId { get; }

        int Stars { get; }
        string HotelName { get; }
        Guid HotelBookingId { get; }
    }
}