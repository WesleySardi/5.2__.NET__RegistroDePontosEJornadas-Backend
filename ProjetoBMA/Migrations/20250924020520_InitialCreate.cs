using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjetoBMA.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TimeEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Type = table.Column<int>(type: "int", maxLength: 20, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeEntries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_EmployeeId",
                table: "TimeEntries",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_Timestamp",
                table: "TimeEntries",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_TimeEntries_Type",
                table: "TimeEntries",
                column: "Type");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeEntries");
        }
    }
}
