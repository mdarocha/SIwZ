using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace server.PostgreSQL
{
    public class TrainSystemContext : DbContext
    {
        public TrainSystemContext(DbContextOptions<TrainSystemContext> options) : base(options)
        {
        }
        
        public DbSet<Train> Trains { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<TrainStop> TrainStops { get; set; }
        public DbSet<Route> Routes { get; set; }
        
        public DbSet<StopToRoute> StopsToRoutes { get; set; }
        public DbSet<Ride> Rides { get; set; }
        
        public DbSet<Ticket> Tickets { get; set; }
        
    }
}