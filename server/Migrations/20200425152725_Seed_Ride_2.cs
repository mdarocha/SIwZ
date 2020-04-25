using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Server.Migrations
{
    public partial class Seed_Ride_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Rides",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Price", "StartTime" },
                values: new object[] { 100, new DateTime(2020, 4, 25, 17, 27, 25, 429, DateTimeKind.Local).AddTicks(140) });

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Price", "StartTime" },
                values: new object[] { 50, new DateTime(2020, 4, 25, 17, 27, 25, 429, DateTimeKind.Local).AddTicks(2011) });

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Price", "StartTime" },
                values: new object[] { 10, new DateTime(2020, 4, 25, 17, 27, 25, 429, DateTimeKind.Local).AddTicks(2050) });

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Price", "StartTime" },
                values: new object[] { 80, new DateTime(2020, 4, 25, 17, 27, 25, 429, DateTimeKind.Local).AddTicks(2054) });

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 1, 5 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 27, 25, 424, DateTimeKind.Local).AddTicks(5900));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 1, 6 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 27, 25, 428, DateTimeKind.Local).AddTicks(7771));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 1, 7 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 27, 25, 428, DateTimeKind.Local).AddTicks(7727));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 2, 1 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 27, 25, 428, DateTimeKind.Local).AddTicks(7777));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 2, 4 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 27, 25, 428, DateTimeKind.Local).AddTicks(7781));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 3, 1 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 27, 25, 428, DateTimeKind.Local).AddTicks(7789));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 3, 2 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 27, 25, 428, DateTimeKind.Local).AddTicks(7785));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 3, 3 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 27, 25, 428, DateTimeKind.Local).AddTicks(7792));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 4, 1 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 27, 25, 428, DateTimeKind.Local).AddTicks(7803));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 4, 2 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 27, 25, 428, DateTimeKind.Local).AddTicks(7799));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 4, 4 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 27, 25, 428, DateTimeKind.Local).AddTicks(7806));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 4, 5 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 27, 25, 428, DateTimeKind.Local).AddTicks(7796));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Rides");

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(5905));

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(7456));

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 3,
                column: "StartTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(7503));

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 4,
                column: "StartTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(7508));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 1, 5 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 356, DateTimeKind.Local).AddTicks(1192));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 1, 6 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(2997));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 1, 7 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(2946));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 2, 1 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(3004));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 2, 4 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(3008));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 3, 1 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(3017));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 3, 2 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(3012));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 3, 3 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(3021));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 4, 1 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(3033));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 4, 2 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(3029));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 4, 4 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(3037));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 4, 5 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(3025));
        }
    }
}
