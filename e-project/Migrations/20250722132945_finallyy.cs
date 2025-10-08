using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_project.Migrations
{
    /// <inheritdoc />
    public partial class finallyy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PrintSizePrices",
                table: "PrintSizePrices");

            migrationBuilder.RenameTable(
                name: "PrintSizePrices",
                newName: "PrintSizePrice");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrintSizePrice",
                table: "PrintSizePrice",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_PrintSizePrice",
                table: "PrintSizePrice");

            migrationBuilder.RenameTable(
                name: "PrintSizePrice",
                newName: "PrintSizePrices");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PrintSizePrices",
                table: "PrintSizePrices",
                column: "Id");
        }
    }
}
