using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Barabas.Migrations
{
    /// <inheritdoc />
    public partial class addCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventCategoryId",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventCategoryId",
                table: "Events",
                column: "EventCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_EventCategories_EventCategoryId",
                table: "Events",
                column: "EventCategoryId",
                principalTable: "EventCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_EventCategories_EventCategoryId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_EventCategoryId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "EventCategoryId",
                table: "Events");
        }
    }
}
