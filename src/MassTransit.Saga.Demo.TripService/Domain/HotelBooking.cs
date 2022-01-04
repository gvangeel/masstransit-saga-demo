using System;

// ReSharper disable AutoPropertyCanBeMadeGetOnly.Local
// ReSharper disable UnusedMember.Local

namespace MassTransit.Saga.Demo.TripService.Domain
{
    /// <summary>
    /// 
    /// </summary>
    public class HotelBooking
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid HotelBookingId { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public int HotelStars { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public string HotelName { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private HotelBooking()
        {
            // Required by EFCore
        }

        internal HotelBooking(Guid hotelBookingId, int hotelStars, string hotelName)
        {
            HotelBookingId = hotelBookingId;
            HotelStars = hotelStars;
            HotelName = hotelName;
        }
    }
}
