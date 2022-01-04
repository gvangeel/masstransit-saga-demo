using MassTransit.Saga.Demo.TripService.Domain;
using Microsoft.EntityFrameworkCore;

namespace MassTransit.Saga.Demo.TripService.Persistence
{
    /// <summary>
    /// 
    /// </summary>
    public class TripDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public TripDbContext(DbContextOptions options)
            : base(options)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TripEntityConfiguration());
            modelBuilder.ApplyConfiguration(new FlightBookingEntityConfiguration());
            modelBuilder.ApplyConfiguration(new HotelBookingEntityConfiguration());
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Trip> TripStates { get; set; }
    }
}
