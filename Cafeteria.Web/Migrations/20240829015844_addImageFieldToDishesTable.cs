using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cafeteria.Web.Migrations
{
    /// <inheritdoc />
    public partial class addImageFieldToDishesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Dishes",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Dishes");
        }
    }
}
