using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class addincreasedogma : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsIncreaseDogma",
                table: "ReadyProducts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIncreaseDogma",
                table: "ReadyProducts");
        }
    }
}
