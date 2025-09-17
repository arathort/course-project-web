using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Barabas.Migrations
{
    /// <inheritdoc />
    public partial class addPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Items");

            migrationBuilder.AddColumn<float>(
                name: "Price",
                table: "Events",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "TicketsCount",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TicketsCount",
                table: "Events");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Items",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
