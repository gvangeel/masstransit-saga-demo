using MassTransit.EntityFrameworkCoreIntegration.Audit;
using MassTransit.Saga.Demo.TripService.Domain;
using Microsoft.EntityFrameworkCore;

namespace MassTransit.Saga.Demo.TripService.Persistence;

public class TripDbContext : DbContext
{
    public TripDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TripEntityConfiguration());
        modelBuilder.ApplyConfiguration(new FlightBookingEntityConfiguration());
        modelBuilder.ApplyConfiguration(new HotelBookingEntityConfiguration());
        modelBuilder.ApplyConfiguration(new AuditMapping("Audit"));
    }

    public DbSet<Trip> TripStates { get; set; }
}