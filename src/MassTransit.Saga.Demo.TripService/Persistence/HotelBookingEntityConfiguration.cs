using MassTransit.Saga.Demo.TripService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MassTransit.Saga.Demo.TripService.Persistence;

public class HotelBookingEntityConfiguration : IEntityTypeConfiguration<HotelBooking>
{
    public void Configure(EntityTypeBuilder<HotelBooking> builder)
    {
        builder.HasKey(_ => _.HotelBookingId);
        builder.Property(c => c.HotelBookingId).ValueGeneratedNever();
    }
}