using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class AddQueueTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Queues",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    VisitDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    ReviewDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    Payment = table.Column<int>(type: "int", nullable: false),
                    NotPaymentReasonId = table.Column<int>(type: "int", nullable: true),
                    Remaining = table.Column<int>(type: "int", nullable: false, computedColumnSql: "(case when [NotPaymentReasonId] IS NULL then isnull([Cost]-[Payment],(0)) else (0) end)", stored: false),
                    VisitStatus = table.Column<byte>(type: "tinyint", nullable: false),
                    VisitIndex = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MedicalReport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Queues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Queues_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Queues_PatientId",
                table: "Queues",
                column: "PatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Queues");
        }
    }
}
