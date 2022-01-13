using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MassTransit.Saga.Demo.HotelService.Migrations
{
    public partial class AddConfirmed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Confirmed",
                table: "HotelBookings",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "HotelBookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "TripId",
                table: "HotelBookings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Confirmed",
                table: "HotelBookings");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "HotelBookings");

            migrationBuilder.DropColumn(
                name: "TripId",
                table: "HotelBookings");
        }
    }
}
