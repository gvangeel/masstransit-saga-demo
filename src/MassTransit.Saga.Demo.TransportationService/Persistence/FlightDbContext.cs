using MassTransit.Saga.Demo.TransportationService.Domain;
using Microsoft.EntityFrameworkCore;

namespace MassTransit.Saga.Demo.TransportationService.Persistence
{
    public class FlightDbContext : DbContext
    {
        public FlightDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { ;
            modelBuilder.ApplyConfiguration(new FlightBookingEntityConfiguration());
        }

        public DbSet<FlightBooking> FlightBookings { get; set; }
    }
}
