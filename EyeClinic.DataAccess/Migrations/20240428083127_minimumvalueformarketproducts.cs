using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class minimumvalueformarketproducts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarketsEntries_MarketsProducts_MarketsProductsId",
                table: "MarketsEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_MarketsExtries_MarketsProducts_MarketsProductsId",
                table: "MarketsExtries");

            migrationBuilder.AddColumn<double>(
                name: "TheMinimumValue",
                table: "MarketsProducts",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<int>(
                name: "MarketsProductsId",
                table: "MarketsExtries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "MarketsProductsId",
                table: "MarketsEntries",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MarketsEntries_MarketsProducts_MarketsProductsId",
                table: "MarketsEntries",
                column: "MarketsProductsId",
                principalTable: "MarketsProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MarketsExtries_MarketsProducts_MarketsProductsId",
                table: "MarketsExtries",
                column: "MarketsProductsId",
                principalTable: "MarketsProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MarketsEntries_MarketsProducts_MarketsProductsId",
                table: "MarketsEntries");

            migrationBuilder.DropForeignKey(
                name: "FK_MarketsExtries_MarketsProducts_MarketsProductsId",
                table: "MarketsExtries");

            migrationBuilder.DropColumn(
                name: "TheMinimumValue",
                table: "MarketsProducts");

            migrationBuilder.AlterColumn<int>(
                name: "MarketsProductsId",
                table: "MarketsExtries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "MarketsProductsId",
                table: "MarketsEntries",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_MarketsEntries_MarketsProducts_MarketsProductsId",
                table: "MarketsEntries",
                column: "MarketsProductsId",
                principalTable: "MarketsProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MarketsExtries_MarketsProducts_MarketsProductsId",
                table: "MarketsExtries",
                column: "MarketsProductsId",
                principalTable: "MarketsProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
