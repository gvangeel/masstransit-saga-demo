using MassTransit.Saga.Demo.Contracts.Trips;

namespace MassTransit.Saga.Demo.HotelService.Domain
{
    public class HotelBooking
    {
        public Guid HotelBookingId { get; private set; }
        public int HotelStars { get; private set; }
        public string HotelName { get; private set; }

        public Guid TripId { get; private set; }

        public string Location { get; private set; }

        public bool Confirmed { get; private set; }

        private HotelBooking()  
        {
            // Required by EFCore
        }

        internal void ConfirmBooking(ITripAllFlightsBooked message)
        {
            Confirmed = true;
        }

        internal HotelBooking(Guid hotelBookingId, int hotelStars, string hotelName, string location, Guid tripId)
        {
            HotelBookingId = hotelBookingId;
            HotelStars = hotelStars;
            HotelName = hotelName;
            TripId = tripId;
            Location = location;
        }
    }
}
