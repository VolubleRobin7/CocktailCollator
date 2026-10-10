using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CocktailCollator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RecipeNotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Recipe",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "RecipeNote",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeNote", x => new { x.UserId, x.RecipeId });
                    table.ForeignKey(
                        name: "FK_RecipeNote_Recipe_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipe",
                        principalColumn: "RecipeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeNote_RecipeId",
                table: "RecipeNote",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipeNote");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Recipe");
        }
    }
}
