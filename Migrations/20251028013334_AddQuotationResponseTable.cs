using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterportCargo.Migrations
{
    /// <inheritdoc />
    public partial class AddQuotationResponseTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QuotationResponses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    QuotationRequestId = table.Column<int>(type: "INTEGER", nullable: false),
                    OfficerId = table.Column<int>(type: "INTEGER", nullable: false),
                    OfficerName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    QuotationNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Message = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false, defaultValueSql: "datetime('now')"),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationResponses_QuotationRequests_QuotationRequestId",
                        column: x => x.QuotationRequestId,
                        principalTable: "QuotationRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_QuotationResponses_QuotationRequestId",
                table: "QuotationResponses",
                column: "QuotationRequestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QuotationResponses");
        }
    }
}
