using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class WareHouseReadyProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WareHouseReadyMaterialId",
                table: "PatientVisitsTests",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WareHouseReadyMaterials",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Quintity = table.Column<double>(type: "float", nullable: false),
                    Unit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitValue = table.Column<double>(type: "float", nullable: false),
                    TotalResult = table.Column<double>(type: "float", nullable: false),
                    FirstTermBalance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WasteValue = table.Column<double>(type: "float", nullable: false),
                    IsProduct = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WareHouseReadyMaterials", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitsTests_WareHouseReadyMaterialId",
                table: "PatientVisitsTests",
                column: "WareHouseReadyMaterialId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVisitsTests_WareHouseReadyMaterials_WareHouseReadyMaterialId",
                table: "PatientVisitsTests",
                column: "WareHouseReadyMaterialId",
                principalTable: "WareHouseReadyMaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientVisitsTests_WareHouseReadyMaterials_WareHouseReadyMaterialId",
                table: "PatientVisitsTests");

            migrationBuilder.DropTable(
                name: "WareHouseReadyMaterials");

            migrationBuilder.DropIndex(
                name: "IX_PatientVisitsTests_WareHouseReadyMaterialId",
                table: "PatientVisitsTests");

            migrationBuilder.DropColumn(
                name: "WareHouseReadyMaterialId",
                table: "PatientVisitsTests");
        }
    }
}
