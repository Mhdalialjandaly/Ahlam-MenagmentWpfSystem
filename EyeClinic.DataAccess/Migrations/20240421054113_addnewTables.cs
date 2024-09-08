using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class addnewTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublicCustomerId",
                table: "Queues",
                type: "int",
                nullable: true);

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

            migrationBuilder.CreateTable(
                name: "PublicCustomers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    MartialStatusId = table.Column<int>(type: "int", nullable: false),
                    HomePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    IsSmoking = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Referral = table.Column<bool>(type: "bit", nullable: false),
                    TempId = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicCustomers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicCustomers_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerNoteStories",
                columns: table => new
                {
                    PublicCustomerId = table.Column<int>(type: "int", nullable: false),
                    NoteStory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerNoteStories", x => x.PublicCustomerId);
                    table.ForeignKey(
                        name: "FK_CustomerNoteStories_PublicCustomers_PublicCustomerId",
                        column: x => x.PublicCustomerId,
                        principalTable: "PublicCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PublicCustomerOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PubliCustomerId = table.Column<int>(type: "int", nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpComingDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OrderStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    OrderIndex = table.Column<int>(type: "int", nullable: false),
                    QueueIndex = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Why = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublicCustomerOrderType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PublicCustomerId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicCustomerOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicCustomerOrders_PublicCustomers_PublicCustomerId",
                        column: x => x.PublicCustomerId,
                        principalTable: "PublicCustomers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Queues_PublicCustomerId",
                table: "Queues",
                column: "PublicCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitsTests_PublicCustomerOrderId",
                table: "PatientVisitsTests",
                column: "PublicCustomerOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitPrescriptions_PublicCustomerOrderId",
                table: "PatientVisitPrescriptions",
                column: "PublicCustomerOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicCustomerOrders_PublicCustomerId",
                table: "PublicCustomerOrders",
                column: "PublicCustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicCustomers_LocationId",
                table: "PublicCustomers",
                column: "LocationId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Queues_PublicCustomers_PublicCustomerId",
                table: "Queues",
                column: "PublicCustomerId",
                principalTable: "PublicCustomers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PatientVisitPrescriptions_PublicCustomerOrders_PublicCustomerOrderId",
                table: "PatientVisitPrescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_PatientVisitsTests_PublicCustomerOrders_PublicCustomerOrderId",
                table: "PatientVisitsTests");

            migrationBuilder.DropForeignKey(
                name: "FK_Queues_PublicCustomers_PublicCustomerId",
                table: "Queues");

            migrationBuilder.DropTable(
                name: "CustomerNoteStories");

            migrationBuilder.DropTable(
                name: "PublicCustomerOrders");

            migrationBuilder.DropTable(
                name: "PublicCustomers");

            migrationBuilder.DropIndex(
                name: "IX_Queues_PublicCustomerId",
                table: "Queues");

            migrationBuilder.DropIndex(
                name: "IX_PatientVisitsTests_PublicCustomerOrderId",
                table: "PatientVisitsTests");

            migrationBuilder.DropIndex(
                name: "IX_PatientVisitPrescriptions_PublicCustomerOrderId",
                table: "PatientVisitPrescriptions");

            migrationBuilder.DropColumn(
                name: "PublicCustomerId",
                table: "Queues");

            migrationBuilder.DropColumn(
                name: "PublicCustomerOrderId",
                table: "PatientVisitsTests");

            migrationBuilder.DropColumn(
                name: "PublicCustomerOrderId",
                table: "PatientVisitPrescriptions");
        }
    }
}
