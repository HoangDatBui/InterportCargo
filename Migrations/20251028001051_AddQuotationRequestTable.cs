using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterportCargo.Migrations
{
    /// <inheritdoc />
    public partial class AddQuotationRequestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuotationRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RequestId = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    CustomerName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CustomerEmail = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Source = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Destination = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    NumberOfContainers = table.Column<int>(type: "INTEGER", nullable: false),
                    NatureOfPackage = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PackageWidth = table.Column<decimal>(type: "TEXT", nullable: false),
                    PackageHeight = table.Column<decimal>(type: "TEXT", nullable: false),
                    PackageDepth = table.Column<decimal>(type: "TEXT", nullable: true),
                    ImportOrExport = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    PackingOrUnpacking = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    IsQuarantineRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    QuarantineDetails = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    IsFumigationRequired = table.Column<bool>(type: "INTEGER", nullable: false),
                    FumigationDetails = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    AdditionalRequirements = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false, defaultValue: "Pending")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationRequests", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuotationRequests");
        }
    }
}
