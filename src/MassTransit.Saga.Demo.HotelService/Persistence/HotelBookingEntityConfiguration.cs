using MassTransit.Saga.Demo.HotelService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MassTransit.Saga.Demo.HotelService.Persistence
{
    public class HotelBookingEntityConfiguration : IEntityTypeConfiguration<HotelBooking>
    {
        public void Configure(EntityTypeBuilder<HotelBooking> builder)
        {
            builder.HasKey(_ => _.HotelBookingId);
            builder.Property(c => c.HotelBookingId).ValueGeneratedNever();
        }
    }
}
