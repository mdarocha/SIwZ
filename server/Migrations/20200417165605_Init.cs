using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace server.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(nullable: false),
                    Value = table.Column<int>(nullable: false),
                    ValueType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Trains",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Seats = table.Column<int>(nullable: false),
                    Wagons = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TrainStops",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    City = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrainStops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Login = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Surname = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Token = table.Column<string>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rides",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RouteId = table.Column<int>(nullable: true),
                    StartTime = table.Column<DateTime>(nullable: false),
                    TrainId = table.Column<int>(nullable: true),
                    FreeTickets = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rides_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rides_Trains_TrainId",
                        column: x => x.TrainId,
                        principalTable: "Trains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StopsToRoutes",
                columns: table => new
                {
                    RouteId = table.Column<int>(nullable: false),
                    TrainStopId = table.Column<int>(nullable: false),
                    StopNo = table.Column<int>(nullable: false),
                    ArrivalTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StopsToRoutes", x => new { x.RouteId, x.TrainStopId });
                    table.ForeignKey(
                        name: "FK_StopsToRoutes_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StopsToRoutes_TrainStops_TrainStopId",
                        column: x => x.TrainStopId,
                        principalTable: "TrainStops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RideId = table.Column<int>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    DiscountId = table.Column<int>(nullable: false),
                    UserId = table.Column<int>(nullable: false),
                    FromId = table.Column<int>(nullable: false),
                    ToId = table.Column<int>(nullable: false),
                    TrainName = table.Column<string>(nullable: false),
                    WagonNo = table.Column<int>(nullable: false),
                    SeatNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_TrainStops_FromId",
                        column: x => x.FromId,
                        principalTable: "TrainStops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Rides_RideId",
                        column: x => x.RideId,
                        principalTable: "Rides",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_TrainStops_ToId",
                        column: x => x.ToId,
                        principalTable: "TrainStops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Tickets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Discounts",
                columns: new[] { "Id", "Type", "Value", "ValueType" },
                values: new object[,]
                {
                    { 1, "ExampleFlat", 2, 1 },
                    { 2, "ExamplePercentage", 5, 0 }
                });

            migrationBuilder.InsertData(
                table: "Routes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Rzeszow-Wroclaw" },
                    { 2, "Krakow-Warszawa" },
                    { 3, "Plaszow-Airport" },
                    { 4, "Rzeszow-Warsaw" }
                });

            migrationBuilder.InsertData(
                table: "TrainStops",
                columns: new[] { "Id", "City", "Name" },
                values: new object[,]
                {
                    { 5, "Rzeszow", "Main train station" },
                    { 4, "Warsaw", "Main train station" },
                    { 3, "Cracow", "Airport" },
                    { 2, "Cracow", "Plaszow" },
                    { 7, "Katowice", "Main train station" },
                    { 6, "Wrocalw", "Main train station" },
                    { 1, "Cracow", "Main train station" }
                });

            migrationBuilder.InsertData(
                table: "Trains",
                columns: new[] { "Id", "Name", "Seats", "Type", "Wagons" },
                values: new object[,]
                {
                    { 4, "REG2", 40, 1, 5 },
                    { 2, "ICC2", 40, 0, 10 },
                    { 1, "ICC1", 60, 0, 5 },
                    { 3, "REG1", 30, 1, 5 }
                });

            migrationBuilder.InsertData(
                table: "StopsToRoutes",
                columns: new[] { "RouteId", "TrainStopId", "ArrivalTime", "StopNo" },
                values: new object[,]
                {
                    { 2, 1, new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2315), 1 },
                    { 3, 1, new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2325), 1 },
                    { 4, 1, new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2339), 1 },
                    { 3, 2, new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2322), 1 },
                    { 4, 2, new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2336), 1 },
                    { 3, 3, new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2329), 1 },
                    { 2, 4, new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2318), 1 },
                    { 4, 4, new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2343), 1 },
                    { 1, 5, new DateTime(2020, 4, 17, 18, 56, 4, 823, DateTimeKind.Local).AddTicks(7475), 1 },
                    { 4, 5, new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2332), 1 },
                    { 1, 6, new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2309), 1 },
                    { 1, 7, new DateTime(2020, 4, 17, 18, 56, 4, 828, DateTimeKind.Local).AddTicks(2265), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rides_RouteId",
                table: "Rides",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_TrainId",
                table: "Rides",
                column: "TrainId");

            migrationBuilder.CreateIndex(
                name: "IX_StopsToRoutes_TrainStopId",
                table: "StopsToRoutes",
                column: "TrainStopId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_DiscountId",
                table: "Tickets",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_FromId",
                table: "Tickets",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_RideId",
                table: "Tickets",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ToId",
                table: "Tickets",
                column: "ToId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_UserId",
                table: "Tickets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StopsToRoutes");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Discounts");

            migrationBuilder.DropTable(
                name: "TrainStops");

            migrationBuilder.DropTable(
                name: "Rides");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Trains");
        }
    }
}
