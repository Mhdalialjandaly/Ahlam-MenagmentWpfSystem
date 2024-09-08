using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder) {
            return;

            migrationBuilder.Sql(
                "Create FUNCTION dbo.GetTotalContactAccountPayments (@Id INT,@PayoutTransaction BIT) RETURNS INT AS BEGIN RETURN (SELECT SUM(PaymentValue) FROM dbo.ContactAccountPayment WHERE DeletedDate IS NULL AND PayoutTransaction = @PayoutTransaction AND ContactAccountId = @Id); END");
            migrationBuilder.Sql(
                "Create FUNCTION dbo.GetTotalPaymentByPaymentTypeId (@PaymentTypeId INT)RETURNS INT AS BEGIN RETURN (SELECT ISNULL(SUM(PaymentValue), 0) AS TotalPayments FROM dbo.Payments WHERE PaymentTypeId = @PaymentTypeId AND dbo.Payments.Paid = 1);END");
            migrationBuilder.Sql(
                "Create FUNCTION dbo.GetTotalPaymentsByPatientOperationId (@PatientOperationId INT)RETURNS INT AS BEGIN RETURN (SELECT ISNULL(SUM(PaymentValue),0) FROM dbo.PatientOperationSessions WHERE PatientOperationId = @PatientOperationId);END");

            migrationBuilder.CreateTable(
                name: "AccountPaymentCategories",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AccountPaymentCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppLanguage",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LanguageCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeviceName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrinterName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    WaitingNext = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "('0001-01-01T00:00:00.0000000')"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_AppLanguage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    ContactPhones = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Diagnosis",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    ArName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Diagnosis", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Diseases",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ArName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Diseases", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EyeTests",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    ArName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_EyeTests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Glasses",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ArName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Glasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LabTests",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LabTestName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_LabTests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ArName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MartialStatus",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ArName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_MartialStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicalCenters",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    ArName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_MedicalCenters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MedicineTypes",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    ArName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_MedicineTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotPayReasons",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ArName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_NotPayReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OldPatientEyeImages",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_OldPatientEyeImages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Operations",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    EnName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ArName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PatientVisitEyeTestHistory",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false),
                    PatientVisitId = table.Column<int>(type: "int", nullable: false),
                    EyeTestId = table.Column<int>(type: "int", nullable: false),
                    LeftEyeResult = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RightEyeResult = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table => {
                });

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    BeneficiaryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TotalCost = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Debt = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "(CONVERT([bit],(0)))"),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remaining = table.Column<int>(type: "int", nullable: false, computedColumnSql: "(isnull([TotalCost]-[dbo].[GetTotalPaymentByPaymentTypeId]([Id]),(0)))", stored: false),
                    TotalPayments = table.Column<int>(type: "int", nullable: true, computedColumnSql: "([dbo].[GetTotalPaymentByPaymentTypeId]([Id]))", stored: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_PaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payouts",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "date", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Payouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReadyPrescriptions",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    ArName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    Disabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_ReadyPrescriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reminders",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReminderText = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false),
                    ReminderDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Reminders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    ArName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tests",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    ArName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Tests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VisitType",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EnName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Cost = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_VisitType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactAccounts",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactId = table.Column<int>(type: "int", nullable: false),
                    AccountName = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: false),
                    TotalCost = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PayOutAccount = table.Column<bool>(type: "bit", nullable: false),
                    Remaining = table.Column<int>(type: "int", nullable: false, computedColumnSql: "([TotalCost]-isnull([dbo].[GetTotalContactAccountPayments]([Id],[PayOutAccount]),(0)))", stored: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_ContactAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactAccounts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Age = table.Column<int>(type: "int", nullable: false),
                    AgeTypeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Gender = table.Column<bool>(type: "bit", nullable: false),
                    MartialStatusId = table.Column<int>(type: "int", nullable: false),
                    HomePhone = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    WorkPhone = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LocationId = table.Column<int>(type: "int", nullable: false),
                    IsSmoking = table.Column<bool>(type: "bit", nullable: false),
                    IsDrinking = table.Column<bool>(type: "bit", nullable: false),
                    IsDrawing = table.Column<bool>(type: "bit", nullable: false),
                    GlassId = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Referral = table.Column<bool>(type: "bit", nullable: false),
                    TempId = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Patients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Patients_Glasses_GlassId",
                        column: x => x.GlassId,
                        principalTable: "Glasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patients_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Patients_MartialStatus_MartialStatusId",
                        column: x => x.MartialStatusId,
                        principalTable: "MartialStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Medicines",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    MedicineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicineTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Medicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Medicines_MedicineTypes_MedicineTypeId",
                        column: x => x.MedicineTypeId,
                        principalTable: "MedicineTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MedicineUsages",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UsageName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UsageMedicineTypeId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_MedicineUsages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicineUsages_MedicineTypes_UsageMedicineTypeId",
                        column: x => x.UsageMedicineTypeId,
                        principalTable: "MedicineTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentTypeId = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    ReminderDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    PaymentValue = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Paid = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "((1))"),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_PaymentTypes_PaymentTypeId",
                        column: x => x.PaymentTypeId,
                        principalTable: "PaymentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactAccountPayment",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactAccountId = table.Column<int>(type: "int", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "(getdate())"),
                    PaymentValue = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ContactId = table.Column<int>(type: "int", nullable: false),
                    PayoutTransaction = table.Column<bool>(type: "bit", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_ContactAccountPayment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactAccountPayment_ContactAccountPaymentCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "AccountPaymentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactAccountPayment_ContactAccounts_ContactAccountId",
                        column: x => x.ContactAccountId,
                        principalTable: "ContactAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ContactAccountPayment_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientDiseases",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    DiseaseId = table.Column<int>(type: "int", nullable: false),
                    AgeOfInjury = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    MaxMeasure = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LastCheckMeasure = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_PatientDiseases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientDiseases_Diseases_DiseaseId",
                        column: x => x.DiseaseId,
                        principalTable: "Diseases",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientDiseases_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PatientGlass",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    R_Sph = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_Sph = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_Sph2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_Sph2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_Cyl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_Cyl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_Cyl2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_Cyl2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_Axis = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_Axis = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_Axis2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_Axis2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_Prism = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_Prism = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_Prism2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_Prism2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_Base = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    L_Base = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    R_Base2 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    L_Base2 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    R_IPD = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_IPD = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_IPD2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_IPD2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_VA = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_VA = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_VA2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_VA2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Add_Vision = table.Column<bool>(type: "bit", nullable: false),
                    ContactLenses = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_PatientGlass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientGlass_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientOperations",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    OperationDate = table.Column<DateTime>(type: "date", nullable: false),
                    LeftEyeOperationId = table.Column<int>(type: "int", nullable: true),
                    RightEyeOperationId = table.Column<int>(type: "int", nullable: true),
                    MedicalCenterId = table.Column<int>(type: "int", nullable: false),
                    MedicalCenterReserved = table.Column<bool>(type: "bit", nullable: false),
                    TotalSessions = table.Column<int>(type: "int", nullable: false),
                    PhotoSource = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PaymentLocation = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    TotalCost = table.Column<int>(type: "int", nullable: false),
                    LeftEyeNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RightEyeNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeftEyeLens = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeftEyeLensType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RightEyeLens = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RightEyeLensType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Report = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFinish = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CenterCost = table.Column<int>(type: "int", nullable: false),
                    ClinicCost = table.Column<int>(type: "int", nullable: false),
                    DownPayment = table.Column<int>(type: "int", nullable: true, defaultValueSql: "((0))"),
                    Revenue = table.Column<int>(type: "int", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Remaining = table.Column<int>(type: "int", nullable: false, computedColumnSql: "(isnull(([TotalCost]-[DownPayment])-[dbo].[GetTotalPaymentsByPatientOperationId]([Id]),(0)))", stored: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_PatientOperations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientOperations_MedicalCenters_MedicalCenterId",
                        column: x => x.MedicalCenterId,
                        principalTable: "MedicalCenters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientOperations_Operations_LeftEyeOperationId",
                        column: x => x.LeftEyeOperationId,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientOperations_Operations_RightEyeOperationId",
                        column: x => x.RightEyeOperationId,
                        principalTable: "Operations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientOperations_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientSickStory",
                columns: table => new {
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    SickStory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_PatientSickStory_PatientId", x => x.PatientId);
                    table.ForeignKey(
                        name: "FK_PatientSickStory_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientVisits",
                columns: table => new {
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
                constraints: table => {
                    table.PrimaryKey("PK_PatientVisits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientVisits_NotPayReasons_Id",
                        column: x => x.NotPaymentReasonId,
                        principalTable: "NotPayReasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientVisits_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReadyPrescriptionMedicines",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReadyPrescriptionId = table.Column<int>(type: "int", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    MedicineUsageId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_ReadyPrescriptionMedicines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReadyPrescriptionMedicines_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReadyPrescriptionMedicines_MedicineUsages",
                        column: x => x.MedicineUsageId,
                        principalTable: "MedicineUsages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReadyPrescriptionMedicines_ReadyPrescriptions_ReadyPrescriptionId",
                        column: x => x.ReadyPrescriptionId,
                        principalTable: "ReadyPrescriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientOperationSessions",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientOperationId = table.Column<int>(type: "int", nullable: false),
                    SessionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    PaymentValue = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SessionNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table => {
                    table.PrimaryKey("PK_PatientOperationSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientOperationSessions_PatientOperations_PatientOperationId",
                        column: x => x.PatientOperationId,
                        principalTable: "PatientOperations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OldMedicineViewTable",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientVisitId = table.Column<int>(type: "int", nullable: false),
                    VisitDate = table.Column<DateTime>(type: "date", nullable: false),
                    MedicineName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MedicineType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    _index = table.Column<int>(type: "int", nullable: true),
                    TempPrescriptionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_OldMedicineViewTable", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OldMedicineViewTable_PatientVisit_PatientVisitId",
                        column: x => x.PatientVisitId,
                        principalTable: "PatientVisits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientVisitDiagnosis",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientVisitId = table.Column<int>(type: "int", nullable: false),
                    DiagnosisId = table.Column<int>(type: "int", nullable: false),
                    LeftEye = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_PatientVisitDiagnosis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientVisitDiagnosis_Diagnosis_DiagnosisId",
                        column: x => x.DiagnosisId,
                        principalTable: "Diagnosis",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientVisitDiagnosis_PatientVisits_PatientVisitId",
                        column: x => x.PatientVisitId,
                        principalTable: "PatientVisits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientVisitEyeTests",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientVisitId = table.Column<int>(type: "int", nullable: false),
                    EyeTestId = table.Column<int>(type: "int", nullable: false),
                    LeftEyeResult = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    RightEyeResult = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_PatientVisitEyeTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientVisitEyeTests_EyeTests_EyeTestId",
                        column: x => x.EyeTestId,
                        principalTable: "EyeTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientVisitEyeTests_PatientVisits_PatientVisitId",
                        column: x => x.PatientVisitId,
                        principalTable: "PatientVisits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientVisitGlass",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientVisitId = table.Column<int>(type: "int", nullable: false),
                    R_Sph = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_Sph = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_Sph2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_Sph2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_Cyl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_Cyl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_Cyl2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_Cyl2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_Axis = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_Axis = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_Axis2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_Axis2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_Prism = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_Prism = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_Prism2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_Prism2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_Base = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    L_Base = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    R_Base2 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    L_Base2 = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    R_IPD = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_IPD = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_IPD2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_IPD2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_VA = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_VA = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    R_VA2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    L_VA2 = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Add_Vision = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ContactLenses = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_PatientVisitGlass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientVisitGlass_PatientVisitId",
                        column: x => x.PatientVisitId,
                        principalTable: "PatientVisits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientVisitLabTests",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientVisitId = table.Column<int>(type: "int", nullable: false),
                    LabTestId = table.Column<int>(type: "int", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    Done = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_PatientVisitLabTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientVisitLabTests_LabTestId",
                        column: x => x.LabTestId,
                        principalTable: "LabTests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientVisitLabTests_PatientVisitId",
                        column: x => x.PatientVisitId,
                        principalTable: "PatientVisits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientVisitPrescriptions",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientVisitId = table.Column<int>(type: "int", nullable: false),
                    MedicineId = table.Column<int>(type: "int", nullable: false),
                    MedicineUsageId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    RowIndex = table.Column<int>(type: "int", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_PatientVisitPrescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientVisitPrescriptions_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientVisitPrescriptions_MedicineUsages_UsageId",
                        column: x => x.MedicineUsageId,
                        principalTable: "MedicineUsages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PatientVisitPrescriptions_PatientVisits_PatientVisitId",
                        column: x => x.PatientVisitId,
                        principalTable: "PatientVisits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientVisitsTests",
                columns: table => new {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientVisitId = table.Column<int>(type: "int", nullable: false),
                    TestId = table.Column<int>(type: "int", nullable: false),
                    LeftEye = table.Column<bool>(type: "bit", nullable: false),
                    LeftEyeNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RightEye = table.Column<bool>(type: "bit", nullable: false),
                    RightEyeNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ImageNumber = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Dropped = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "(CONVERT([bit],(0)))"),
                    CostPayment = table.Column<int>(type: "int", nullable: false),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table => {
                    table.PrimaryKey("PK_PatientVisitsTests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PatientVisitsTests_PatientVisits_PatientVisitId",
                        column: x => x.PatientVisitId,
                        principalTable: "PatientVisits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientVisitsTests_Tests_TestId",
                        column: x => x.TestId,
                        principalTable: "Tests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ArName", "CreatedDate", "DeletedBy", "DeletedDate", "EnName", "LastModifiedDate" },
                values: new object[,]
                {
                    { 1, "مسؤول", new DateTime(2022, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Admin", null },
                    { 2, "استقبال", new DateTime(2022, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Reception", null },
                    { 3, "ممرضة", new DateTime(2022, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Nurse", null },
                    { 4, "طبيب", new DateTime(2022, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Doctor", null }
                });

            migrationBuilder.InsertData(
                table: "VisitType",
                columns: new[] { "Id", "ArName", "Cost", "CreatedDate", "DeletedBy", "DeletedDate", "EnName", "LastModifiedDate" },
                values: new object[,]
                {
                    { 1, "مراجعة", 0, new DateTime(2022, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Review", null },
                    { 2, "مراجعة مدفوعة", 10000, new DateTime(2022, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "Payment Review", null },
                    { 3, "اول مرة", 15000, new DateTime(2022, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "First Time", null },
                    { 4, "زيارة جديدة", 15000, new DateTime(2022, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "New Visit", null },
                    { 5, "IOP", 10000, new DateTime(2022, 3, 27, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "IOP", null }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedDate", "DeletedBy", "DeletedDate", "IsActive", "LastModifiedDate", "Password", "RoleId", "UserName" },
                values: new object[] { 1, new DateTime(2022, 1, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, true, null, "1", 1, "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactAccountPayment_CategoryId",
                table: "ContactAccountPayment",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactAccountPayment_ContactAccountId",
                table: "ContactAccountPayment",
                column: "ContactAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactAccountPayment_ContactId",
                table: "ContactAccountPayment",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactAccounts_ContactId",
                table: "ContactAccounts",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Medicines_MedicineTypeId",
                table: "Medicines",
                column: "MedicineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicineUsages_UsageMedicineTypeId",
                table: "MedicineUsages",
                column: "UsageMedicineTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OldMedicineViewTable_PatientVisitId",
                table: "OldMedicineViewTable",
                column: "PatientVisitId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDiseases_DiseaseId",
                table: "PatientDiseases",
                column: "DiseaseId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDiseases_PatientId",
                table: "PatientDiseases",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "Unique_PatientGlass_PatientId",
                table: "PatientGlass",
                column: "PatientId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PatientOperations_LeftEyeOperationId",
                table: "PatientOperations",
                column: "LeftEyeOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientOperations_MedicalCenterId",
                table: "PatientOperations",
                column: "MedicalCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientOperations_PatientId",
                table: "PatientOperations",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientOperations_RightEyeOperationId",
                table: "PatientOperations",
                column: "RightEyeOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientOperationSessions_PatientOperationId",
                table: "PatientOperationSessions",
                column: "PatientOperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_GlassId",
                table: "Patients",
                column: "GlassId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_LocationId",
                table: "Patients",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_MartialStatusId",
                table: "Patients",
                column: "MartialStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitDiagnosis_DiagnosisId",
                table: "PatientVisitDiagnosis",
                column: "DiagnosisId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitDiagnosis_PatientVisitId",
                table: "PatientVisitDiagnosis",
                column: "PatientVisitId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitEyeTests_EyeTestId",
                table: "PatientVisitEyeTests",
                column: "EyeTestId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitEyeTests_PatientVisitId",
                table: "PatientVisitEyeTests",
                column: "PatientVisitId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitGlass_PatientVisitId",
                table: "PatientVisitGlass",
                column: "PatientVisitId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitLabTests_LabTestId",
                table: "PatientVisitLabTests",
                column: "LabTestId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitLabTests_PatientVisitId",
                table: "PatientVisitLabTests",
                column: "PatientVisitId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitPrescriptions_MedicineId",
                table: "PatientVisitPrescriptions",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitPrescriptions_MedicineUsageId",
                table: "PatientVisitPrescriptions",
                column: "MedicineUsageId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitPrescriptions_PatientVisitId",
                table: "PatientVisitPrescriptions",
                column: "PatientVisitId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisits_NotPaymentReasonId",
                table: "PatientVisits",
                column: "NotPaymentReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisits_PatientId",
                table: "PatientVisits",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisits_PatientId_VisitDate",
                table: "PatientVisits",
                column: "VisitDate");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitsTests_PatientVisitId",
                table: "PatientVisitsTests",
                column: "PatientVisitId");

            migrationBuilder.CreateIndex(
                name: "IX_PatientVisitsTests_TestId",
                table: "PatientVisitsTests",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentTypeId",
                table: "Payments",
                column: "PaymentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadyPrescriptionMedicines_MedicineId",
                table: "ReadyPrescriptionMedicines",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadyPrescriptionMedicines_MedicineUsageId",
                table: "ReadyPrescriptionMedicines",
                column: "MedicineUsageId");

            migrationBuilder.CreateIndex(
                name: "IX_ReadyPrescriptionMedicines_ReadyPrescriptionId",
                table: "ReadyPrescriptionMedicines",
                column: "ReadyPrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "Unique_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder) {
            migrationBuilder.DropTable(
                name: "AppLanguage");

            migrationBuilder.DropTable(
                name: "ContactAccountPayment");

            migrationBuilder.DropTable(
                name: "OldMedicineViewTable");

            migrationBuilder.DropTable(
                name: "OldPatientEyeImages");

            migrationBuilder.DropTable(
                name: "PatientDiseases");

            migrationBuilder.DropTable(
                name: "PatientGlass");

            migrationBuilder.DropTable(
                name: "PatientOperationSessions");

            migrationBuilder.DropTable(
                name: "PatientSickStory");

            migrationBuilder.DropTable(
                name: "PatientVisitDiagnosis");

            migrationBuilder.DropTable(
                name: "PatientVisitEyeTestHistory");

            migrationBuilder.DropTable(
                name: "PatientVisitEyeTests");

            migrationBuilder.DropTable(
                name: "PatientVisitGlass");

            migrationBuilder.DropTable(
                name: "PatientVisitLabTests");

            migrationBuilder.DropTable(
                name: "PatientVisitPrescriptions");

            migrationBuilder.DropTable(
                name: "PatientVisitsTests");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Payouts");

            migrationBuilder.DropTable(
                name: "ReadyPrescriptionMedicines");

            migrationBuilder.DropTable(
                name: "Reminders");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VisitType");

            migrationBuilder.DropTable(
                name: "AccountPaymentCategories");

            migrationBuilder.DropTable(
                name: "ContactAccounts");

            migrationBuilder.DropTable(
                name: "Diseases");

            migrationBuilder.DropTable(
                name: "PatientOperations");

            migrationBuilder.DropTable(
                name: "Diagnosis");

            migrationBuilder.DropTable(
                name: "EyeTests");

            migrationBuilder.DropTable(
                name: "LabTests");

            migrationBuilder.DropTable(
                name: "PatientVisits");

            migrationBuilder.DropTable(
                name: "Tests");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropTable(
                name: "Medicines");

            migrationBuilder.DropTable(
                name: "MedicineUsages");

            migrationBuilder.DropTable(
                name: "ReadyPrescriptions");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "MedicalCenters");

            migrationBuilder.DropTable(
                name: "Operations");

            migrationBuilder.DropTable(
                name: "NotPayReasons");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "MedicineTypes");

            migrationBuilder.DropTable(
                name: "Glasses");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "MartialStatus");
        }
    }
}
