using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EyeClinic.DataAccess.Migrations
{
    public partial class isoperation : Migration
    {

        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            // Example: Add a new column  
            migrationBuilder.AddColumn<string>(
                name: "IsOperation",
                table: "Patients",
                type: "binary",
                nullable: true);

      
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {          

            migrationBuilder.AddColumn<string>(
                name: "IsOperation",
                table: "Patients",
                type: "binary",
                nullable: true);
        }
    }
}
