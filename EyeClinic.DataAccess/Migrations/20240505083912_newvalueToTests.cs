using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class newvalueToTests : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "FirstTermBalanceWareHouseValue",
                table: "Tests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalEntryWareHouseValue",
                table: "Tests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalWareHouseValue",
                table: "Tests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalWasteWareHouseValue",
                table: "Tests",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FirstTermBalanceWareHouseValue",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "TotalEntryWareHouseValue",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "TotalWareHouseValue",
                table: "Tests");

            migrationBuilder.DropColumn(
                name: "TotalWasteWareHouseValue",
                table: "Tests");
        }
    }
}
