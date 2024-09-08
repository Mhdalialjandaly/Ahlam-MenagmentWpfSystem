using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class ChangeReservationTimeType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Time",
                table: "Reservations");

            migrationBuilder.AddColumn<string>(
                name: "ReservationTime",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationTime",
                table: "Reservations");

            migrationBuilder.AddColumn<DateTime>(
                name: "Time",
                table: "Reservations",
                type: "datetime2",
                nullable: true);
        }
    }
}
