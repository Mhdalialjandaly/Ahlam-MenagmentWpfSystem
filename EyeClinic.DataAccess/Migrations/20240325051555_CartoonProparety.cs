using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class CartoonProparety : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Note",
                table: "Contacts",
                newName: "note");

            migrationBuilder.AddColumn<string>(
                name: "Enrty",
                table: "Diseases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Extry",
                table: "Diseases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstDayValue",
                table: "Diseases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Diseases",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ThePNumber",
                table: "Diseases",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Enrty",
                table: "Diseases");

            migrationBuilder.DropColumn(
                name: "Extry",
                table: "Diseases");

            migrationBuilder.DropColumn(
                name: "FirstDayValue",
                table: "Diseases");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Diseases");

            migrationBuilder.DropColumn(
                name: "ThePNumber",
                table: "Diseases");

            migrationBuilder.RenameColumn(
                name: "note",
                table: "Contacts",
                newName: "Note");
        }
    }
}
