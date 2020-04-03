using System.Linq;
using System.Threading;
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
        
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TrainStop>().HasData(
                new TrainStop {Id = 1, City = "Cracow", Name = "Main train station"},
                new TrainStop {Id = 2, City = "Cracow", Name = "Plaszow"},
                new TrainStop {Id = 3, City = "Cracow", Name = "Airport"},
                new TrainStop {Id = 4, City = "Warsaw", Name = "Main train station"},
                new TrainStop {Id = 5, City = "Rzeszow", Name = "Main train station"},
                new TrainStop {Id = 6, City = "Wrocalw", Name = "Main train station"},
                new TrainStop {Id = 7, City = "Katowice", Name = "Main train station"}
            );

            modelBuilder.Entity<Route>().HasData(
                new Route {Id = 1, Name = "Rzeszow-Wroclaw"},
                new Route {Id = 2, Name = "Krakow-Warszawa"},
                new Route {Id = 3, Name = "Plaszow-Airport"},
                new Route {Id = 4, Name = "Rzeszow-Warsaw"}
            );
            
            modelBuilder.Entity<Train>().HasData(
                new Train {
                    Id = 1, 
                    Name = "ICC1", 
                    Seats = 60, 
                    Type = (int) Train.TrainTypes.Interstitial,
                    Wagons = 5
                },
                new Train {
                    Id = 2, 
                    Name = "ICC2", 
                    Seats = 40, 
                    Type = (int) Train.TrainTypes.Interstitial,
                    Wagons = 10
                },
            new Train {
                    Id = 3, 
                    Name = "REG1", 
                    Seats = 30, 
                    Type = (int) Train.TrainTypes.NotInterstitial,
                    Wagons = 5
                },
            new Train {
                    Id = 4, 
                    Name = "REG2", 
                    Seats = 40, 
                    Type = (int) Train.TrainTypes.NotInterstitial,
                    Wagons = 5
                }
            );
            
            modelBuilder.Entity<Discount>().HasData(
                new Discount {Id = 1, Type = "ExampleFlat", Value = 2, ValueType = (int) Discount.DiscountValueTypes.Flat},
                new Discount {Id = 2, Type = "ExamplePercentage", Value = 5, ValueType = (int) Discount.DiscountValueTypes.Percentage}
            );

            /*
            TODO: to bee seeded, i wont do it
            modelBuilder.Entity<StopToRoute>().HasData(
                new StopToRoute
                {
                    Id = 1, 
                    RouteId = 1, 
                    TrainStop = 5, 
                    StopNo = 1, 
                    ArrivalTime = new System.DateTime(0, 0, 0, 1, 15, 0),
                },
                new StopToRoute
                {
                    Id = 2, 
                    RouteId = 1, 
                    TrainStop = 7, 
                    StopNo = 1, 
                    ArrivalTime = new System.DateTime(0, 0, 0, 1, 15, 0),
                },
                new StopToRoute
                {
                    Id = 3, 
                    RouteId = 1, 
                    TrainStop = 6, 
                    StopNo = 1, 
                    ArrivalTime = new System.DateTime(0, 0, 0, 1, 15, 0),
                },
                new StopToRoute
                {
                    Id = 4, 
                    RouteId = 2, 
                    TrainStop = 1, 
                    StopNo = 1, 
                    ArrivalTime = new System.DateTime(0, 0, 0, 1, 15, 0),
                },
                new StopToRoute
                {
                    Id = 5, 
                    RouteId = 2, 
                    TrainStop = 4, 
                    StopNo = 1, 
                    ArrivalTime = new System.DateTime(0, 0, 0, 1, 15, 0),
                },
                new StopToRoute
                {
                    Id = 6, 
                    RouteId = 3, 
                    TrainStop = 2, 
                    StopNo = 1, 
                    ArrivalTime = new System.DateTime(0, 0, 0, 1, 15, 0),
                },
                new StopToRoute
                {
                    Id = 7,
                    RouteId = 3, 
                    TrainStop = 1, 
                    StopNo = 1, 
                    ArrivalTime = new System.DateTime(0, 0, 0, 1, 15, 0),
                },
                new StopToRoute
                {
                    Id = 8, 
                    RouteId = 3, 
                    TrainStop = 3, 
                    StopNo = 1, 
                    ArrivalTime = new System.DateTime(0, 0, 0, 1, 15, 0),
                },
                new StopToRoute
                {
                    Id = 9, 
                    RouteId = 4, 
                    TrainStop = 5, 
                    StopNo = 1, 
                    ArrivalTime = new System.DateTime(0, 0, 0, 1, 15, 0),
                },
                new StopToRoute
                {
                    Id = 10, 
                    RouteId = 4, 
                    TrainStop = 2, 
                    StopNo = 1, 
                    ArrivalTime = new System.DateTime(0, 0, 0, 1, 15, 0),
                },
                new StopToRoute
                {
                    Id = 11, 
                    RouteId = 4, 
                    TrainStop = 1, 
                    StopNo = 1, 
                    ArrivalTime = new System.DateTime(0, 0, 0, 1, 15, 0),
                },
                new StopToRoute
                {
                    Id = 12, 
                    RouteId = 4, 
                    TrainStop = 4, 
                    StopNo = 1, 
                    ArrivalTime = new System.DateTime(0, 0, 0, 1, 15, 0),
                }
            );
            */
        }
    }
}