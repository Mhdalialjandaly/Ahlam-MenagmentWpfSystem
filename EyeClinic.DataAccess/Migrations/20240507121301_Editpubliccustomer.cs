using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class Editpubliccustomer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "PublicCustomers");

            migrationBuilder.DropColumn(
                name: "IsSmoking",
                table: "PublicCustomers");

            migrationBuilder.DropColumn(
                name: "JobTitle",
                table: "PublicCustomers");

            migrationBuilder.DropColumn(
                name: "MartialStatusId",
                table: "PublicCustomers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Gender",
                table: "PublicCustomers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsSmoking",
                table: "PublicCustomers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "JobTitle",
                table: "PublicCustomers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MartialStatusId",
                table: "PublicCustomers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
