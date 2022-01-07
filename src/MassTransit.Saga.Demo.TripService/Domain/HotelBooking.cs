namespace MassTransit.Saga.Demo.TripService.Domain;

public class HotelBooking
{
    public Guid HotelBookingId { get; private set; }
    public int HotelStars { get; private set; }
    public string HotelName { get; private set; }

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