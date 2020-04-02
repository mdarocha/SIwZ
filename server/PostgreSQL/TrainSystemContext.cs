using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace server.PostgreSQL
{
    public class TrainSystemContext : DbContext
    {
        public TrainSystemContext(DbContextOptions<TrainSystemContext> options) : base(options)
        {
        }

        public DbSet<TrainStop> TrainStops { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<StopToRoute> StopsToRoutes { get; set; }
        public DbSet<Ride> Rides { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Train> Trains { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StopToRoute>(eb => eb.HasNoKey());
        }
    }
}