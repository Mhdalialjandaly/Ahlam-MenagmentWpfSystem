using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class UpdateRemainingColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            migrationBuilder.Sql("ALTER TABLE dbo.PatientVisits DROP COLUMN Remaining");
            migrationBuilder.Sql("ALTER TABLE dbo.PatientVisits ADD [Remaining] AS (CASE WHEN [NotPaymentReasonId] = 4 THEN ISNULL([Cost] - [Payment], (0)) ELSE (0) END)");
        }

        protected override void Down(MigrationBuilder migrationBuilder) {

        }
    }
}
