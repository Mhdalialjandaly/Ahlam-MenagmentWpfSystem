using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class add_left_right_imagename : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.AddColumn<string>(
                name: "ImageNameLeft",
                table: "PatientVisitsTests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageNameRight",
                table: "PatientVisitsTests",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitsTests_MedicalCenterId",
                table: "PatientVisitsTests",
                column: "MedicalCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVisitsTests_MedicalCenters_MedicalCenterId",
                table: "PatientVisitsTests",
                column: "MedicalCenterId",
                principalTable: "MedicalCenters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.Sql("Update PatientVisitsTests Set ImageNameLeft = ImageName");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientVisitsTests_MedicalCenters_MedicalCenterId",
                table: "PatientVisitsTests");

            migrationBuilder.DropIndex(
                name: "IX_PatientVisitsTests_MedicalCenterId",
                table: "PatientVisitsTests");

            migrationBuilder.DropColumn(
                name: "ImageNameLeft",
                table: "PatientVisitsTests");

            migrationBuilder.DropColumn(
                name: "ImageNameRight",
                table: "PatientVisitsTests");
        }
    }
}
