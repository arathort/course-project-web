using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Barabas.Migrations
{
    /// <inheritdoc />
    public partial class ddrganizererification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerifiedOrganizer",
                table: "Users",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerifiedOrganizer",
                table: "Users");
        }
    }
}
