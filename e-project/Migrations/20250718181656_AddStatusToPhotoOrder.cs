using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace e_project.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToPhotoOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "PhotoOrders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "PhotoOrders");
        }
    }
}
