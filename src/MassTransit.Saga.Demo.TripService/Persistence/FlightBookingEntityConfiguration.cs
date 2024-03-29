﻿using MassTransit.Saga.Demo.TripService.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MassTransit.Saga.Demo.TripService.Persistence;

public class FlightBookingEntityConfiguration : IEntityTypeConfiguration<FlightBooking>
{
    public void Configure(EntityTypeBuilder<FlightBooking> builder)
    {
        builder.HasKey(_ => _.FlightId);
        builder.Property(c => c.FlightId).ValueGeneratedNever();
        builder.Property(c => c.FlightCost).HasPrecision(14,2);
    }
}