using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace server.Migrations
{
    public partial class ManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StopsToRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RouteId = table.Column<int>(nullable: true),
                    TrainStopId = table.Column<string>(nullable: true),
                    StopNo = table.Column<int>(nullable: false),
                    ArrivalTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StopsToRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StopsToRoutes_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StopsToRoutes_TrainStops_TrainStopId",
                        column: x => x.TrainStopId,
                        principalTable: "TrainStops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StopsToRoutes_RouteId",
                table: "StopsToRoutes",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_StopsToRoutes_TrainStopId",
                table: "StopsToRoutes",
                column: "TrainStopId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StopsToRoutes");
        }
    }
}
