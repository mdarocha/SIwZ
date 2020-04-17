﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using server.Database;

namespace server.Migrations
{
    [DbContext(typeof(TrainSystemContext))]
    [Migration("20200417165605_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Server.Models.Discount", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Value")
                        .HasColumnType("integer");

                    b.Property<int>("ValueType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Discounts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Type = "ExampleFlat",
                            Value = 2,
                            ValueType = 1
                        },
                        new
                        {
                            Id = 2,
                            Type = "ExamplePercentage",
                            Value = 5,
                            ValueType = 0
                        });
                });

            modelBuilder.Entity("Server.Models.Ride", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("FreeTickets")
                        .HasColumnType("integer");

                    b.Property<int?>("RouteId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("TrainId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.HasIndex("TrainId");

                    b.ToTable("Rides");
                });

            modelBuilder.Entity("Server.Models.Route", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Routes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Rzeszow-Wroclaw"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Krakow-Warszawa"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Plaszow-Airport"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Rzeszow-Warsaw"
                        });
                });

            modelBuilder.Entity("Server.Models.StopToRoute", b =>
                {
                    b.Property<int>("RouteId")
                        .HasColumnType("integer");

                    b.Property<int>("TrainStopId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ArrivalTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("StopNo")
                        .HasColumnType("integer");

                    b.HasKey("RouteId", "TrainStopId");

                    b.HasIndex("TrainStopId");

                    b.ToTable("StopsToRoutes");

                    b.HasData(
                        new
                        {
                            RouteId = 1,
                            TrainStopId = 5,
                            ArrivalTime = new DateTime(2020, 4, 17, 18, 56, 4, 823, DateTimeKind.Local).AddTicks(7475),
                            StopNo = 1
                        },
                        new
                        {
                            RouteId = 1,
                            TrainStopId = 7,
                            ArrivalTime = new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2265),
                            StopNo = 1
                        },
                        new
                        {
                            RouteId = 1,
                            TrainStopId = 6,
                            ArrivalTime = new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2309),
                            StopNo = 1
                        },
                        new
                        {
                            RouteId = 2,
                            TrainStopId = 1,
                            ArrivalTime = new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2315),
                            StopNo = 1
                        },
                        new
                        {
                            RouteId = 2,
                            TrainStopId = 4,
                            ArrivalTime = new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2318),
                            StopNo = 1
                        },
                        new
                        {
                            RouteId = 3,
                            TrainStopId = 2,
                            ArrivalTime = new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2322),
                            StopNo = 1
                        },
                        new
                        {
                            RouteId = 3,
                            TrainStopId = 1,
                            ArrivalTime = new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2325),
                            StopNo = 1
                        },
                        new
                        {
                            RouteId = 3,
                            TrainStopId = 3,
                            ArrivalTime = new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2329),
                            StopNo = 1
                        },
                        new
                        {
                            RouteId = 4,
                            TrainStopId = 5,
                            ArrivalTime = new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2332),
                            StopNo = 1
                        },
                        new
                        {
                            RouteId = 4,
                            TrainStopId = 2,
                            ArrivalTime = new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2336),
                            StopNo = 1
                        },
                        new
                        {
                            RouteId = 4,
                            TrainStopId = 1,
                            ArrivalTime = new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2339),
                            StopNo = 1
                        },
                        new
                        {
                            RouteId = 4,
                            TrainStopId = 4,
                            ArrivalTime = new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2343),
                            StopNo = 1
                        });
                });

            modelBuilder.Entity("Server.Models.Ticket", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("DiscountId")
                        .HasColumnType("integer");

                    b.Property<int>("FromId")
                        .HasColumnType("integer");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<int>("RideId")
                        .HasColumnType("integer");

                    b.Property<int>("SeatNo")
                        .HasColumnType("integer");

                    b.Property<int>("ToId")
                        .HasColumnType("integer");

                    b.Property<string>("TrainName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("WagonNo")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DiscountId");

                    b.HasIndex("FromId");

                    b.HasIndex("RideId");

                    b.HasIndex("ToId");

                    b.HasIndex("UserId");

                    b.ToTable("Tickets");
                });

            modelBuilder.Entity("Server.Models.Train", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Seats")
                        .HasColumnType("integer");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<int>("Wagons")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Trains");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "ICC1",
                            Seats = 60,
                            Type = 0,
                            Wagons = 5
                        },
                        new
                        {
                            Id = 2,
                            Name = "ICC2",
                            Seats = 40,
                            Type = 0,
                            Wagons = 10
                        },
                        new
                        {
                            Id = 3,
                            Name = "REG1",
                            Seats = 30,
                            Type = 1,
                            Wagons = 5
                        },
                        new
                        {
                            Id = 4,
                            Name = "REG2",
                            Seats = 40,
                            Type = 1,
                            Wagons = 5
                        });
                });

            modelBuilder.Entity("Server.Models.TrainStop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TrainStops");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            City = "Cracow",
                            Name = "Main train station"
                        },
                        new
                        {
                            Id = 2,
                            City = "Cracow",
                            Name = "Plaszow"
                        },
                        new
                        {
                            Id = 3,
                            City = "Cracow",
                            Name = "Airport"
                        },
                        new
                        {
                            Id = 4,
                            City = "Warsaw",
                            Name = "Main train station"
                        },
                        new
                        {
                            Id = 5,
                            City = "Rzeszow",
                            Name = "Main train station"
                        },
                        new
                        {
                            Id = 6,
                            City = "Wrocalw",
                            Name = "Main train station"
                        },
                        new
                        {
                            Id = 7,
                            City = "Katowice",
                            Name = "Main train station"
                        });
                });

            modelBuilder.Entity("Server.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Server.Models.Ride", b =>
                {
                    b.HasOne("Server.Models.Route", "Route")
                        .WithMany()
                        .HasForeignKey("RouteId");

                    b.HasOne("Server.Models.Train", "Train")
                        .WithMany()
                        .HasForeignKey("TrainId");
                });

            modelBuilder.Entity("Server.Models.StopToRoute", b =>
                {
                    b.HasOne("Server.Models.Route", "Route")
                        .WithMany()
                        .HasForeignKey("RouteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.TrainStop", "TrainStop")
                        .WithMany()
                        .HasForeignKey("TrainStopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Server.Models.Ticket", b =>
                {
                    b.HasOne("Server.Models.Discount", "Discount")
                        .WithMany()
                        .HasForeignKey("DiscountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.TrainStop", "From")
                        .WithMany()
                        .HasForeignKey("FromId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.Ride", "Ride")
                        .WithMany()
                        .HasForeignKey("RideId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.TrainStop", "To")
                        .WithMany()
                        .HasForeignKey("ToId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
