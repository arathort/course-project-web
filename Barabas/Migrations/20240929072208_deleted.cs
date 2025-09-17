using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Barabas.Migrations
{
    /// <inheritdoc />
    public partial class deleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "OrderId", table: "OrderItem");
            migrationBuilder.AddColumn<string>(name: "UserId", table: "OrderItem");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "UserId", table: "OrderItem");
            migrationBuilder.AddColumn<int>(name: "OrderId", table: "OrderItem");
        }
    }
}
