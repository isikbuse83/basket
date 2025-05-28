using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsoleApp1.Migrations
{
    /// <inheritdoc />
    public partial class Update_stocks_Basket : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductStock",
                table: "Products",
                newName: "WarehouseStock");

            migrationBuilder.AddColumn<int>(
                name: "DynamicStock",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DynamicStock",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "WarehouseStock",
                table: "Products",
                newName: "ProductStock");
        }
    }
}
