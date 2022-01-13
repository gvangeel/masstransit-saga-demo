using MassTransit.Saga.Demo.Contracts.Trips;
using MassTransit.Saga.Demo.HotelService.Domain;
using MassTransit.Saga.Demo.HotelService.Persistence;

namespace MassTransit.Saga.Demo.HotelService.Application
{
    public class TripRegisteredConsumer: IConsumer<ITripRegistered>
    {
        private readonly HotelDbContext _dbContext;

        public TripRegisteredConsumer(HotelDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Consume(ConsumeContext<ITripRegistered> context)
        {
            var booking = new HotelBooking(NewId.NextGuid(), context.Message.RequiredStars, "Hilton", context.Message.Destination, context.Message.TripId);
            await _dbContext.HotelBookings.AddAsync(booking);
            await _dbContext.SaveChangesAsync(CancellationToken.None);
        }
    }
}
