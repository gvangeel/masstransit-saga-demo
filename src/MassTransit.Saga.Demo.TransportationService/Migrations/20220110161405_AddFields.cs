using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MassTransit.Saga.Demo.TransportationService.Migrations
{
    public partial class AddFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FlightDate",
                table: "FlightBookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "From",
                table: "FlightBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "To",
                table: "FlightBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FlightDate",
                table: "FlightBookings");

            migrationBuilder.DropColumn(
                name: "From",
                table: "FlightBookings");

            migrationBuilder.DropColumn(
                name: "To",
                table: "FlightBookings");
        }
    }
}
