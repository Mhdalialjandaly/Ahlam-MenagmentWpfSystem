using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class PatientPregnantMonth : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<DateTime>(
                name: "EndPregnantDate",
                table: "Patients",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PregnantMonth",
                table: "Patients",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartPregnantDate",
                table: "Patients",
                type: "datetime",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropColumn(
                name: "EndPregnantDate",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "PregnantMonth",
                table: "Patients");

            migrationBuilder.DropColumn(
                name: "StartPregnantDate",
                table: "Patients");
        }
    }
}
