using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class pdftoorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PdfSource",
                table: "PatientVisitsTests",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PdfSource",
                table: "PatientVisitsTests");
        }
    }
}
