using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class MarketsTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MarketsProducts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MarketsProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstTermValue = table.Column<double>(type: "float", nullable: false),
                    Entry = table.Column<double>(type: "float", nullable: false),
                    Extry = table.Column<double>(type: "float", nullable: false),
                    RealValue = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketsProducts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MarketsEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quinttity = table.Column<double>(type: "float", nullable: false),
                    UnitName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeNumber = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MarketsProductsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketsEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketsEntries_MarketsProducts_MarketsProductsId",
                        column: x => x.MarketsProductsId,
                        principalTable: "MarketsProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MarketsExtries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quinttity = table.Column<double>(type: "float", nullable: false),
                    UnitName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Recipter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CodeNumber = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MarketsProductsId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MarketsExtries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MarketsExtries_MarketsProducts_MarketsProductsId",
                        column: x => x.MarketsProductsId,
                        principalTable: "MarketsProducts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MarketsEntries_MarketsProductsId",
                table: "MarketsEntries",
                column: "MarketsProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_MarketsExtries_MarketsProductsId",
                table: "MarketsExtries",
                column: "MarketsProductsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MarketsEntries");

            migrationBuilder.DropTable(
                name: "MarketsExtries");

            migrationBuilder.DropTable(
                name: "MarketsProducts");
        }
    }
}
