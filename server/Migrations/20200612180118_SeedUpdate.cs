using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace server.Migrations
{
    public partial class SeedUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 1,
                column: "StartTime",
                value: new DateTime(2020, 6, 12, 20, 1, 17, 822, DateTimeKind.Local).AddTicks(8220));

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsEveryDayRide", "StartTime" },
                values: new object[] { true, new DateTime(2020, 6, 12, 20, 1, 17, 827, DateTimeKind.Local).AddTicks(6512) });

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 3,
                column: "StartTime",
                value: new DateTime(2020, 6, 12, 20, 1, 17, 827, DateTimeKind.Local).AddTicks(6582));

            migrationBuilder.UpdateData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "IsEveryDayRide", "StartTime" },
                values: new object[] { true, new DateTime(2020, 6, 12, 20, 1, 17, 827, DateTimeKind.Local).AddTicks(6589) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "IsEveryDayRide", "StartTime" },
                values: new object[] { false, new DateTime(2020, 6, 12, 19, 14, 51, 396, DateTimeKind.Local).AddTicks(4297) });

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
                columns: new[] { "IsEveryDayRide", "StartTime" },
                values: new object[] { false, new DateTime(2020, 6, 12, 19, 14, 51, 396, DateTimeKind.Local).AddTicks(4366) });
        }
    }
}
