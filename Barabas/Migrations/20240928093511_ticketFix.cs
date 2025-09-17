using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Barabas.Migrations
{
    /// <inheritdoc />
    public partial class ticketFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketsCount",
                table: "Events");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tickets",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tickets");

            migrationBuilder.AddColumn<int>(
                name: "TicketsCount",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
