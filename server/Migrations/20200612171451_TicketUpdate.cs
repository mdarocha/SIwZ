using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class TicketUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "RideDate",
                table: "Tickets",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsEveryDayRide",
                table: "Rides",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartTime",
                value: new DateTime(2020, 6, 12, 19, 14, 51, 391, DateTimeKind.Local).AddTicks(6030));

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartTime",
                value: new DateTime(2020, 6, 12, 19, 14, 51, 396, DateTimeKind.Local).AddTicks(4297));

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 3,
                column: "StartTime",
                value: new DateTime(2020, 6, 12, 19, 14, 51, 396, DateTimeKind.Local).AddTicks(4359));

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 4,
                column: "StartTime",
                value: new DateTime(2020, 6, 12, 19, 14, 51, 396, DateTimeKind.Local).AddTicks(4366));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RideDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IsEveryDayRide",
                table: "Rides");

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartTime",
                value: new DateTime(2020, 5, 29, 12, 18, 7, 805, DateTimeKind.Local).AddTicks(1254));

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 2,
                column: "StartTime",
                value: new DateTime(2020, 5, 29, 12, 18, 7, 811, DateTimeKind.Local).AddTicks(6563));

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 3,
                column: "StartTime",
                value: new DateTime(2020, 5, 29, 12, 18, 7, 811, DateTimeKind.Local).AddTicks(6651));

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 4,
                column: "StartTime",
                value: new DateTime(2020, 5, 29, 12, 18, 7, 811, DateTimeKind.Local).AddTicks(6661));
        }
    }
}
