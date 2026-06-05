using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CocktailCollator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RecipeCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RecipeCategoryId",
                table: "Recipe",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RecipeCategory",
                columns: table => new
                {
                    RecipeCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeCategory", x => x.RecipeCategoryId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Recipe_RecipeCategoryId",
                table: "Recipe",
                column: "RecipeCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Recipe_RecipeCategory_RecipeCategoryId",
                table: "Recipe",
                column: "RecipeCategoryId",
                principalTable: "RecipeCategory",
                principalColumn: "RecipeCategoryId",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Recipe_RecipeCategory_RecipeCategoryId",
                table: "Recipe");

            migrationBuilder.DropTable(
                name: "RecipeCategory");

            migrationBuilder.DropIndex(
                name: "IX_Recipe_RecipeCategoryId",
                table: "Recipe");

            migrationBuilder.DropColumn(
                name: "RecipeCategoryId",
                table: "Recipe");
        }
    }
}
