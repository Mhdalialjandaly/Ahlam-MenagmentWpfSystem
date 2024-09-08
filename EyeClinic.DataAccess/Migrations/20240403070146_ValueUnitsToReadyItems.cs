using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class ValueUnitsToReadyItems : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedValueUnit",
                table: "ReadyProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExportedValueUnit",
                table: "ReadyProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductedValueUnit",
                table: "ReadyProducts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WasteValueUnit",
                table: "ReadyProducts",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedValueUnit",
                table: "ReadyProducts");

            migrationBuilder.DropColumn(
                name: "ExportedValueUnit",
                table: "ReadyProducts");

            migrationBuilder.DropColumn(
                name: "ProductedValueUnit",
                table: "ReadyProducts");

            migrationBuilder.DropColumn(
                name: "WasteValueUnit",
                table: "ReadyProducts");
        }
    }
}
