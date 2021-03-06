using System;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Server.Models;

namespace Server.Database
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
                new TrainStop { Id = 1, City = "Cracow", Name = "Main train station"},
                new TrainStop { Id = 2, City = "Cracow", Name = "Plaszow"},
                new TrainStop { Id = 3, City = "Cracow", Name = "Airport"},
                new TrainStop { Id = 4, City = "Warsaw", Name = "Main train station"},
                new TrainStop { Id = 5, City = "Rzeszow", Name = "Main train station"},
                new TrainStop { Id = 6, City = "Wrocalw", Name = "Main train station"},
                new TrainStop { Id = 7, City = "Katowice", Name = "Main train station"}
            );
            modelBuilder.Entity<TrainStop>().Property(p => p.Id).HasIdentityOptions(startValue: 8);
            
            modelBuilder.Entity<Route>().HasData(
                new Route { Id = 1, Name = "Rzeszow-Wroclaw"},
                new Route { Id = 2, Name = "Krakow-Warszawa"},
                new Route { Id = 3, Name = "Plaszow-Airport"},
                new Route { Id = 4, Name = "Rzeszow-Warsaw"}
            );
            modelBuilder.Entity<Route>().Property(p => p.Id).HasIdentityOptions(startValue: 5);
            
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
            modelBuilder.Entity<Train>().Property(p => p.Id).HasIdentityOptions(startValue: 5);
            
            modelBuilder.Entity<Discount>().HasData(
                new Discount
                {
                    Id = 1,
                    Type = "NoDiscount", 
                    Value = 0, 
                    ValueType = Discount.DiscountValueTypes.Flat
                },
                new Discount
                {
                    Id = 2,
                    Type = "ExampleFlat", 
                    Value = 2, 
                    ValueType = Discount.DiscountValueTypes.Flat
                },
                new Discount
                {
                    Id = 3,
                    Type = "ExamplePercentage", 
                    Value = 5,
                    ValueType = Discount.DiscountValueTypes.Percentage
                }
            );
            modelBuilder.Entity<Discount>().Property(p => p.Id).HasIdentityOptions(startValue: 4);
            
            modelBuilder.Entity<StopToRoute>().HasData(
                new StopToRoute
                {
                    RouteId = 1,
                    TrainStopId = 5,
                    StopNo = 1,
                    HoursDiff = 0,
                    MinutesDiff = 0
                },
                new StopToRoute
                {
                    RouteId = 1,
                    TrainStopId = 7,
                    StopNo = 2,
                    HoursDiff = 0,
                    MinutesDiff = 26
                },
                new StopToRoute
                {
                    RouteId = 1,
                    TrainStopId = 6,
                    StopNo = 3,
                    HoursDiff = 0,
                    MinutesDiff = 47
                },
                new StopToRoute
                {
                    RouteId = 2,
                    TrainStopId = 1,
                    StopNo = 1,
                    HoursDiff = 0,
                    MinutesDiff = 0
                },
                new StopToRoute
                {
                    RouteId = 2,
                    TrainStopId = 4,
                    StopNo = 2,
                    HoursDiff = 0,
                    MinutesDiff = 17
                },
                new StopToRoute
                {
                    RouteId = 3,
                    TrainStopId = 2,
                    StopNo = 1,
                    HoursDiff = 0,
                    MinutesDiff = 0
                },
                new StopToRoute
                {
                    RouteId = 3,
                    TrainStopId = 1,
                    StopNo = 2,
                    HoursDiff = 0,
                    MinutesDiff = 37
                },
                new StopToRoute
                {
                    RouteId = 3,
                    TrainStopId = 3,
                    StopNo = 3,
                    HoursDiff = 0,
                    MinutesDiff = 54
                },
                new StopToRoute
                {
                    RouteId = 4,
                    TrainStopId = 5,
                    StopNo = 1,
                    HoursDiff = 0,
                    MinutesDiff = 0
                },
                new StopToRoute
                {
                    RouteId = 4,
                    TrainStopId = 2,
                    StopNo = 2,
                    HoursDiff = 0,
                    MinutesDiff = 43
                },
                new StopToRoute
                {
                    RouteId = 4,
                    TrainStopId = 1,
                    StopNo = 3,
                    HoursDiff = 1,
                    MinutesDiff = 12
                },
                new StopToRoute
                {
                    RouteId = 4,
                    TrainStopId = 4,
                    StopNo = 4,
                    HoursDiff = 0,
                    MinutesDiff = 27
                }
            );

            modelBuilder.Entity<Ride>().HasData(
                new Ride
                {
                    Id = 1,
                    RouteId = 1,
                    StartTime = DateTime.Now,
                    TrainId = 1,
                    Price = 100,
                    IsEveryDayRide = false
                },
                new Ride
                {
                    Id = 2,
                    RouteId = 2,
                    StartTime = DateTime.Now,
                    TrainId = 2,
                    Price = 50,
                    IsEveryDayRide = true,
                }, 
                new Ride
                {
                    Id = 3,
                    RouteId = 3,
                    StartTime = DateTime.Now,
                    TrainId = 3,
                    Price = 10,
                    IsEveryDayRide = false
                    
                }, 
                new Ride
                {
                    Id = 4,
                    RouteId = 4,
                    StartTime = DateTime.Now,
                    TrainId = 4,
                    Price = 80,
                    IsEveryDayRide = true
                }
            );
            modelBuilder.Entity<Ride>().Property(p => p.Id).HasIdentityOptions(startValue: 5);
        }
    }
}