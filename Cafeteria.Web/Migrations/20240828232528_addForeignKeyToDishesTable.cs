using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cafeteria.Web.Migrations
{
    /// <inheritdoc />
    public partial class addForeignKeyToDishesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "Dishes");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Dishes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "Dishes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_CategoryId",
                table: "Dishes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Dishes_CurrencyId",
                table: "Dishes",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Category_CategoryId",
                table: "Dishes",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dishes_Currencies_CurrencyId",
                table: "Dishes",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Category_CategoryId",
                table: "Dishes");

            migrationBuilder.DropForeignKey(
                name: "FK_Dishes_Currencies_CurrencyId",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_CategoryId",
                table: "Dishes");

            migrationBuilder.DropIndex(
                name: "IX_Dishes_CurrencyId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Dishes");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "Dishes");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "Dishes",
                type: "nvarchar(3)",
                maxLength: 3,
                nullable: false,
                defaultValue: "");
        }
    }
}
