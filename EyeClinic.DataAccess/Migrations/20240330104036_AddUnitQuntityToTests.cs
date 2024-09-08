using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class AddUnitQuntityToTests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DFirstDayValue",
                table: "Diseases");

            migrationBuilder.AddColumn<int>(
                name: "Quintity",
                table: "Tests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "Tests",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quintity",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "Tests");

            migrationBuilder.AddColumn<double>(
                name: "DFirstDayValue",
                table: "Diseases",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
