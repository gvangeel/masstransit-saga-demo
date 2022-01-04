using MassTransit.Saga.Demo.TripService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MassTransit.Saga.Demo.TripService.Persistence
{
    /// <summary>
    /// 
    /// </summary>
    public class HotelBookingEntityConfiguration : IEntityTypeConfiguration<HotelBooking>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<HotelBooking> builder)
        {
            builder.HasKey(_ => _.HotelBookingId);
            builder.Property(c => c.HotelBookingId).ValueGeneratedNever();
        }
    }
}