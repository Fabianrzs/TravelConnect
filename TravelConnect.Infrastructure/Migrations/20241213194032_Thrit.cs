using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TravelConnect.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Thrit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_EmergencyContactId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Reservations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_EmergencyContactId",
                table: "Reservations",
                column: "EmergencyContactId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reservations_EmergencyContactId",
                table: "Reservations");

            migrationBuilder.AddColumn<Guid>(
                name: "HotelId",
                table: "Reservations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_EmergencyContactId",
                table: "Reservations",
                column: "EmergencyContactId");
        }
    }
}
