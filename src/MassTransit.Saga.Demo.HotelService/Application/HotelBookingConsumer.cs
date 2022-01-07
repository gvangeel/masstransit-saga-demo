using System;
using System.Threading.Tasks;
using MassTransit.Saga.Demo.Contracts.Hotels;

namespace MassTransit.Saga.Demo.HotelService.Application
{
    public class HotelBookingConsumer : IConsumer<IBookHotelRequest>
    {
        public async Task Consume(ConsumeContext<IBookHotelRequest> context)
        {
            await Task.Delay(TimeSpan.FromSeconds(15));
            await context.Publish<IHotelBooked>(new
            {
                context.Message.TripId,
                Stars = context.Message.RequiredStars,
                HotelName = "Hilton",
                HotelBookingId = NewId.NextGuid()
            });
        }
    }
}
