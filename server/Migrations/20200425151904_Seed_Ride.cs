using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Server.Migrations
{
    public partial class Seed_Ride : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Routes_RouteId",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Trains_TrainId",
                table: "Rides");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TrainStops",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:IdentitySequenceOptions", "'8', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Trains",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:IdentitySequenceOptions", "'5', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Routes",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:IdentitySequenceOptions", "'5', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "TrainId",
                table: "Rides",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RouteId",
                table: "Rides",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Rides",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:IdentitySequenceOptions", "'5', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Discounts",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:IdentitySequenceOptions", "'3', '1', '', '', 'False', '1'")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.InsertData(
                table: "Rides",
                columns: new[] { "Id", "FreeTickets", "RouteId", "StartTime", "TrainId" },
                values: new object[,]
                {
                    { 1, 300, 1, new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(5905), 1 },
                    { 2, 400, 2, new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(7456), 2 },
                    { 3, 150, 3, new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(7503), 3 },
                    { 4, 200, 4, new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(7508), 4 }
                });

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
                columns: new[] { "ArrivalTime", "StopNo" },
                values: new object[] { new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(2997), 3 });

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 1, 7 },
                columns: new[] { "ArrivalTime", "StopNo" },
                values: new object[] { new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(2946), 2 });

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
                columns: new[] { "ArrivalTime", "StopNo" },
                values: new object[] { new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(3008), 2 });

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 3, 1 },
                columns: new[] { "ArrivalTime", "StopNo" },
                values: new object[] { new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(3017), 2 });

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
                columns: new[] { "ArrivalTime", "StopNo" },
                values: new object[] { new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(3021), 3 });

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 4, 1 },
                columns: new[] { "ArrivalTime", "StopNo" },
                values: new object[] { new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(3033), 3 });

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 4, 2 },
                columns: new[] { "ArrivalTime", "StopNo" },
                values: new object[] { new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(3029), 2 });

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 4, 4 },
                columns: new[] { "ArrivalTime", "StopNo" },
                values: new object[] { new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(3037), 4 });

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 4, 5 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 25, 17, 19, 4, 361, DateTimeKind.Local).AddTicks(3025));

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Routes_RouteId",
                table: "Rides",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Trains_TrainId",
                table: "Rides",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Routes_RouteId",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Trains_TrainId",
                table: "Rides");

            migrationBuilder.DeleteData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Rides",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TrainStops",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'8', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Trains",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'5', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Routes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'5', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "TrainId",
                table: "Rides",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "RouteId",
                table: "Rides",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Rides",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'5', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Discounts",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int))
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .OldAnnotation("Npgsql:IdentitySequenceOptions", "'3', '1', '', '', 'False', '1'")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 1, 5 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 17, 20, 53, 49, 177, DateTimeKind.Local).AddTicks(2126));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 1, 6 },
                columns: new[] { "ArrivalTime", "StopNo" },
                values: new object[] { new DateTime(2020, 4, 17, 20, 53, 49, 182, DateTimeKind.Local).AddTicks(1172), 1 });

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 1, 7 },
                columns: new[] { "ArrivalTime", "StopNo" },
                values: new object[] { new DateTime(2020, 4, 17, 20, 53, 49, 182, DateTimeKind.Local).AddTicks(1125), 1 });

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 2, 1 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 17, 20, 53, 49, 182, DateTimeKind.Local).AddTicks(1178));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 2, 4 },
                columns: new[] { "ArrivalTime", "StopNo" },
                values: new object[] { new DateTime(2020, 4, 17, 20, 53, 49, 182, DateTimeKind.Local).AddTicks(1182), 1 });

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 3, 1 },
                columns: new[] { "ArrivalTime", "StopNo" },
                values: new object[] { new DateTime(2020, 4, 17, 20, 53, 49, 182, DateTimeKind.Local).AddTicks(1189), 1 });

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 3, 2 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 17, 20, 53, 49, 182, DateTimeKind.Local).AddTicks(1186));

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 3, 3 },
                columns: new[] { "ArrivalTime", "StopNo" },
                values: new object[] { new DateTime(2020, 4, 17, 20, 53, 49, 182, DateTimeKind.Local).AddTicks(1193), 1 });

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 4, 1 },
                columns: new[] { "ArrivalTime", "StopNo" },
                values: new object[] { new DateTime(2020, 4, 17, 20, 53, 49, 182, DateTimeKind.Local).AddTicks(1205), 1 });

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 4, 2 },
                columns: new[] { "ArrivalTime", "StopNo" },
                values: new object[] { new DateTime(2020, 4, 17, 20, 53, 49, 182, DateTimeKind.Local).AddTicks(1201), 1 });

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 4, 4 },
                columns: new[] { "ArrivalTime", "StopNo" },
                values: new object[] { new DateTime(2020, 4, 17, 20, 53, 49, 182, DateTimeKind.Local).AddTicks(1209), 1 });

            migrationBuilder.UpdateData(
                table: "StopsToRoutes",
                keyColumns: new[] { "RouteId", "TrainStopId" },
                keyValues: new object[] { 4, 5 },
                column: "ArrivalTime",
                value: new DateTime(2020, 4, 17, 20, 53, 49, 182, DateTimeKind.Local).AddTicks(1197));

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Routes_RouteId",
                table: "Rides",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Trains_TrainId",
                table: "Rides",
                column: "TrainId",
                principalTable: "Trains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
