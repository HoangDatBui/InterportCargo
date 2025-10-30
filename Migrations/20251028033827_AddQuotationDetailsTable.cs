using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterportCargo.Migrations
{
    /// <inheritdoc />
    public partial class AddQuotationDetailsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuotationDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuotationNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    QuotationRequestId = table.Column<int>(type: "INTEGER", nullable: false),
                    OfficerId = table.Column<int>(type: "INTEGER", nullable: false),
                    OfficerName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    DateIssued = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ContainerType = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Scope = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    Subtotal = table.Column<decimal>(type: "TEXT", nullable: false),
                    DiscountPercentage = table.Column<decimal>(type: "TEXT", nullable: false),
                    DiscountAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    AmountAfterDiscount = table.Column<decimal>(type: "TEXT", nullable: false),
                    GST = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    ItemizedCharges = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false, defaultValue: "Pending"),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationDetails_QuotationRequests_QuotationRequestId",
                        column: x => x.QuotationRequestId,
                        principalTable: "QuotationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuotationDetails_QuotationRequestId",
                table: "QuotationDetails",
                column: "QuotationRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuotationDetails");
        }
    }
}
