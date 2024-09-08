using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class EditPublicCustomerTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicCustomerOrders_PublicCustomers_PublicCustomerId",
                table: "PublicCustomerOrders");

            migrationBuilder.DropColumn(
                name: "PubliCustomerId",
                table: "PublicCustomerOrders");

            migrationBuilder.AlterColumn<int>(
                name: "PublicCustomerId",
                table: "PublicCustomerOrders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicCustomerOrders_PublicCustomers_PublicCustomerId",
                table: "PublicCustomerOrders",
                column: "PublicCustomerId",
                principalTable: "PublicCustomers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PublicCustomerOrders_PublicCustomers_PublicCustomerId",
                table: "PublicCustomerOrders");

            migrationBuilder.AlterColumn<int>(
                name: "PublicCustomerId",
                table: "PublicCustomerOrders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PubliCustomerId",
                table: "PublicCustomerOrders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_PublicCustomerOrders_PublicCustomers_PublicCustomerId",
                table: "PublicCustomerOrders",
                column: "PublicCustomerId",
                principalTable: "PublicCustomers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
