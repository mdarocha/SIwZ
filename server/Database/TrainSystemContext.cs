using System;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace server.Database
{
    public class TrainSystemContext : IdentityDbContext<User, Role, string>
    {
        public TrainSystemContext(DbContextOptions<TrainSystemContext> options) : base(options)
        {
        }

        public DbSet<Train> Trains { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<TrainStop> TrainStops { get; set; }
        public DbSet<Route> Routes { get; set; }

        public DbSet<StopToRoute> StopsToRoutes { get; set; }
        public DbSet<Ride> Rides { get; set; }

        public DbSet<Ticket> Tickets { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<StopToRoute>()
                .HasKey(x => new {x.RouteId, x.TrainStopId});
            
            Seed(modelBuilder); //sneed
            
            base.OnModelCreating(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
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
                new Train
                {
                    Id = 1,
                    Name = "ICC1",
                    Seats = 60,
                    Type = Train.TrainType.Sectional,
                    Wagons = 5
                },
                new Train
                {
                    Id = 2,
                    Name = "ICC2",
                    Seats = 40,
                    Type = Train.TrainType.Sectional,
                    Wagons = 10
                },
                new Train
                {
                    Id = 3,
                    Name = "REG1",
                    Seats = 30,
                    Type = Train.TrainType.OpenCoach,
                    Wagons = 5
                },
                new Train
                {
                    Id = 4,
                    Name = "REG2",
                    Seats = 40,
                    Type = Train.TrainType.OpenCoach,
                    Wagons = 5
                }
            );

            modelBuilder.Entity<Discount>().HasData(
                new Discount
                {
                    Id = 1,
                    Type = "ExampleFlat", 
                    Value = 2, 
                    ValueType = Discount.DiscountValueTypes.Flat
                },
                new Discount
                {
                    Id = 2, Type = "ExamplePercentage", Value = 5,
                    ValueType = Discount.DiscountValueTypes.Percentage
                }
            );

            modelBuilder.Entity<StopToRoute>().HasData(
                new StopToRoute
                {
                    RouteId = 1,
                    TrainStopId = 5,
                    StopNo = 1,
                    ArrivalTime = DateTime.Now
                },
                new StopToRoute
                {
                    RouteId = 1,
                    TrainStopId = 7,
                    StopNo = 1,
                    ArrivalTime = DateTime.Now
                },
                new StopToRoute
                {
                    RouteId = 1,
                    TrainStopId = 6,
                    StopNo = 1,
                    ArrivalTime = DateTime.Now
                },
                new StopToRoute
                {
                    RouteId = 2,
                    TrainStopId = 1,
                    StopNo = 1,
                    ArrivalTime = DateTime.Now
                },
                new StopToRoute
                {
                    RouteId = 2,
                    TrainStopId = 4,
                    StopNo = 1,
                    ArrivalTime = DateTime.Now
                },
                new StopToRoute
                {
                    RouteId = 3,
                    TrainStopId = 2,
                    StopNo = 1,
                    ArrivalTime = DateTime.Now
                },
                new StopToRoute
                {
                    RouteId = 3,
                    TrainStopId = 1,
                    StopNo = 1,
                    ArrivalTime = DateTime.Now
                },
                new StopToRoute
                {
                    RouteId = 3,
                    TrainStopId = 3,
                    StopNo = 1,
                    ArrivalTime = DateTime.Now
                },
                new StopToRoute
                {
                    RouteId = 4,
                    TrainStopId = 5,
                    StopNo = 1,
                    ArrivalTime = DateTime.Now
                },
                new StopToRoute
                {
                    RouteId = 4,
                    TrainStopId = 2,
                    StopNo = 1,
                    ArrivalTime = DateTime.Now
                },
                new StopToRoute
                {
                    RouteId = 4,
                    TrainStopId = 1,
                    StopNo = 1,
                    ArrivalTime = DateTime.Now
                },
                new StopToRoute
                {
                    RouteId = 4,
                    TrainStopId = 4,
                    StopNo = 1,
                    ArrivalTime = DateTime.Now
                }
            );
        }
    }
}