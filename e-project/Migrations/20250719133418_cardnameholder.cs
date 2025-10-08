using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_project.Migrations
{
    /// <inheritdoc />
    public partial class cardnameholder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "CreditCardNumber",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CardholderName",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ShippingAddress",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardholderName",
                table: "PhotoOrders");

            migrationBuilder.DropColumn(
                name: "ShippingAddress",
                table: "PhotoOrders");

            migrationBuilder.AlterColumn<string>(
                name: "CreditCardNumber",
                table: "PhotoOrders",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
