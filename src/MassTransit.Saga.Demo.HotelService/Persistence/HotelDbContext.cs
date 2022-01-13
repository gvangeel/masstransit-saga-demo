using MassTransit.Saga.Demo.HotelService.Domain;
using Microsoft.EntityFrameworkCore;

namespace MassTransit.Saga.Demo.HotelService.Persistence
{
    public class HotelDbContext : DbContext
    {
        public HotelDbContext(DbContextOptions options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        { ;
            modelBuilder.ApplyConfiguration(new HotelBookingEntityConfiguration());
        }

        public DbSet<HotelBooking> HotelBookings { get; set; }
    }
}
