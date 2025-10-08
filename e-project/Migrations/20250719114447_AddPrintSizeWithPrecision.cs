using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace e_project.Migrations
{
    /// <inheritdoc />
    public partial class AddPrintSizeWithPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PrintSizePrices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Size = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrintSizePrices", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "PrintSizePrices",
                columns: new[] { "Id", "Price", "Size" },
                values: new object[,]
                {
                    { 1, 10.00m, "4x6" },
                    { 2, 15.00m, "5x7" },
                    { 3, 25.00m, "8x10" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PrintSizePrices");
        }
    }
}
