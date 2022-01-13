using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MassTransit.Saga.Demo.TransportationService.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlightBookings",
                columns: table => new
                {
                    FlightId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsOutboundFlight = table.Column<bool>(type: "bit", nullable: false),
                    FlightCost = table.Column<decimal>(type: "decimal(14,2)", precision: 14, scale: 2, nullable: false),
                    AirlineCompany = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TripId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightBookings", x => x.FlightId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlightBookings");
        }
    }
}
