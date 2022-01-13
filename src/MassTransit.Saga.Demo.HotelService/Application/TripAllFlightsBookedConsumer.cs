using MassTransit.Saga.Demo.Contracts.Hotels;
using MassTransit.Saga.Demo.Contracts.Trips;
using MassTransit.Saga.Demo.HotelService.Persistence;
using Microsoft.EntityFrameworkCore;

namespace MassTransit.Saga.Demo.HotelService.Application;

public class TripAllFlightsBookedConsumer : IConsumer<ITripAllFlightsBooked>
{
    private readonly HotelDbContext _dbcontext;

    public TripAllFlightsBookedConsumer(HotelDbContext context)
    {
        _dbcontext = context;
    }

    public async Task Consume(ConsumeContext<ITripAllFlightsBooked> context)
    {
        await Task.Delay(TimeSpan.FromSeconds(15));
        var booking = await _dbcontext.HotelBookings.FirstAsync(hotelBooking =>
            hotelBooking.TripId == context.Message.TripId);
        booking.ConfirmBooking(context.Message);
        await _dbcontext.SaveChangesAsync(CancellationToken.None);
        await context.Publish<IHotelBooked>(new
        {
            context.Message.TripId,
            Stars = booking.HotelStars,
            booking.HotelName,
            booking.HotelBookingId
        });
    }
}