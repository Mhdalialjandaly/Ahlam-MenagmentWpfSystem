using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class OperationsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientVisitPrescriptions_PublicCustomerOrders_PublicCustomerOrderId",
                table: "PatientVisitPrescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientVisitsTests_PublicCustomerOrders_PublicCustomerOrderId",
                table: "PatientVisitsTests");

            migrationBuilder.DropIndex(
                name: "IX_PatientVisitsTests_PublicCustomerOrderId",
                table: "PatientVisitsTests");

            migrationBuilder.DropIndex(
                name: "IX_PatientVisitPrescriptions_PublicCustomerOrderId",
                table: "PatientVisitPrescriptions");

            migrationBuilder.DropColumn(
                name: "PublicCustomerOrderId",
                table: "PatientVisitsTests");

            migrationBuilder.DropColumn(
                name: "PublicCustomerOrderId",
                table: "PatientVisitPrescriptions");

            migrationBuilder.CreateTable(
                name: "ProductDepartment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDepartment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReadyProducted",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Disabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadyProducted", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductDepartmentId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Product_ProductDepartment_ProductDepartmentId",
                        column: x => x.ProductDepartmentId,
                        principalTable: "ProductDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductDescription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DescriptionName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductDescriptionDepartmentId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DescriptionProductDepartmentId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductDescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductDescription_ProductDepartment_DescriptionProductDepartmentId",
                        column: x => x.DescriptionProductDepartmentId,
                        principalTable: "ProductDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PublicCustomerOrderProduct",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PublicCustomerOrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductDescriptionId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RowIndex = table.Column<int>(type: "int", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicCustomerOrderProduct", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicCustomerOrderProduct_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicCustomerOrderProduct_ProductDescription_ProductDescriptionId",
                        column: x => x.ProductDescriptionId,
                        principalTable: "ProductDescription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PublicCustomerOrderProduct_PublicCustomerOrders_PublicCustomerOrderId",
                        column: x => x.PublicCustomerOrderId,
                        principalTable: "PublicCustomerOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReadyProductOrder",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReadyProductedId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    ProductDescriptionId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReadyProductOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadyProductOrder_Product_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Product",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReadyProductOrder_ProductDescription_ProductDescriptionId",
                        column: x => x.ProductDescriptionId,
                        principalTable: "ProductDescription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReadyProductOrder_ReadyProducted_ReadyProductedId",
                        column: x => x.ReadyProductedId,
                        principalTable: "ReadyProducted",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_ProductDepartmentId",
                table: "Product",
                column: "ProductDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductDescription_DescriptionProductDepartmentId",
                table: "ProductDescription",
                column: "DescriptionProductDepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicCustomerOrderProduct_ProductDescriptionId",
                table: "PublicCustomerOrderProduct",
                column: "ProductDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicCustomerOrderProduct_ProductId",
                table: "PublicCustomerOrderProduct",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicCustomerOrderProduct_PublicCustomerOrderId",
                table: "PublicCustomerOrderProduct",
                column: "PublicCustomerOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadyProductOrder_ProductDescriptionId",
                table: "ReadyProductOrder",
                column: "ProductDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadyProductOrder_ProductId",
                table: "ReadyProductOrder",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadyProductOrder_ReadyProductedId",
                table: "ReadyProductOrder",
                column: "ReadyProductedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PublicCustomerOrderProduct");

            migrationBuilder.DropTable(
                name: "ReadyProductOrder");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "ProductDescription");

            migrationBuilder.DropTable(
                name: "ReadyProducted");

            migrationBuilder.DropTable(
                name: "ProductDepartment");

            migrationBuilder.AddColumn<int>(
                name: "PublicCustomerOrderId",
                table: "PatientVisitsTests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PublicCustomerOrderId",
                table: "PatientVisitPrescriptions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitsTests_PublicCustomerOrderId",
                table: "PatientVisitsTests",
                column: "PublicCustomerOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitPrescriptions_PublicCustomerOrderId",
                table: "PatientVisitPrescriptions",
                column: "PublicCustomerOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVisitPrescriptions_PublicCustomerOrders_PublicCustomerOrderId",
                table: "PatientVisitPrescriptions",
                column: "PublicCustomerOrderId",
                principalTable: "PublicCustomerOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PatientVisitsTests_PublicCustomerOrders_PublicCustomerOrderId",
                table: "PatientVisitsTests",
                column: "PublicCustomerOrderId",
                principalTable: "PublicCustomerOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
