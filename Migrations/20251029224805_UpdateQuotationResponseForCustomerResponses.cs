using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InterportCargo.Migrations
{
    /// <inheritdoc />
    public partial class UpdateQuotationResponseForCustomerResponses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OfficerName",
                table: "QuotationResponses",
                type: "TEXT",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<int>(
                name: "OfficerId",
                table: "QuotationResponses",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "CustomerId",
                table: "QuotationResponses",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "QuotationResponses",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ResponseType",
                table: "QuotationResponses",
                type: "TEXT",
                maxLength: 20,
                nullable: false,
                defaultValue: "Officer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "QuotationResponses");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "QuotationResponses");

            migrationBuilder.DropColumn(
                name: "ResponseType",
                table: "QuotationResponses");

            migrationBuilder.AlterColumn<string>(
                name: "OfficerName",
                table: "QuotationResponses",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OfficerId",
                table: "QuotationResponses",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
