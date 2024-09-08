using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class WareHouseReadyProductTestId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestId",
                table: "WareHouseReadyMaterials",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_WareHouseReadyMaterials_TestId",
                table: "WareHouseReadyMaterials",
                column: "TestId");

            migrationBuilder.AddForeignKey(
                name: "FK_WareHouseReadyMaterials_Tests_TestId",
                table: "WareHouseReadyMaterials",
                column: "TestId",
                principalTable: "Tests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WareHouseReadyMaterials_Tests_TestId",
                table: "WareHouseReadyMaterials");

            migrationBuilder.DropIndex(
                name: "IX_WareHouseReadyMaterials_TestId",
                table: "WareHouseReadyMaterials");

            migrationBuilder.DropColumn(
                name: "TestId",
                table: "WareHouseReadyMaterials");
        }
    }
}
