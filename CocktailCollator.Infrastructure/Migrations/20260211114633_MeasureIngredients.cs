using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CocktailCollator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MeasureIngredients : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Amount",
                table: "RecipeIngredient",
                type: "decimal(9,4)",
                precision: 9,
                scale: 4,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "MeasurementId",
                table: "RecipeIngredient",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "IngredientCategoryId",
                table: "Ingredient",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IngredientCategory",
                columns: table => new
                {
                    IngredientCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientCategory", x => x.IngredientCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Measurement",
                columns: table => new
                {
                    MeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Measurement", x => x.MeasurementId);
                });

            migrationBuilder.CreateTable(
                name: "IngredientMeasurement",
                columns: table => new
                {
                    IngredientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeasurementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IngredientMeasurement", x => new { x.IngredientId, x.MeasurementId });
                    table.ForeignKey(
                        name: "FK_IngredientMeasurement_Ingredient_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "Ingredient",
                        principalColumn: "IngredientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IngredientMeasurement_Measurement_MeasurementId",
                        column: x => x.MeasurementId,
                        principalTable: "Measurement",
                        principalColumn: "MeasurementId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipeIngredient_MeasurementId",
                table: "RecipeIngredient",
                column: "MeasurementId");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredient_IngredientCategoryId",
                table: "Ingredient",
                column: "IngredientCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_IngredientMeasurement_MeasurementId",
                table: "IngredientMeasurement",
                column: "MeasurementId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredient_IngredientCategory_IngredientCategoryId",
                table: "Ingredient",
                column: "IngredientCategoryId",
                principalTable: "IngredientCategory",
                principalColumn: "IngredientCategoryId",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_RecipeIngredient_Measurement_MeasurementId",
                table: "RecipeIngredient",
                column: "MeasurementId",
                principalTable: "Measurement",
                principalColumn: "MeasurementId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredient_IngredientCategory_IngredientCategoryId",
                table: "Ingredient");

            migrationBuilder.DropForeignKey(
                name: "FK_RecipeIngredient_Measurement_MeasurementId",
                table: "RecipeIngredient");

            migrationBuilder.DropTable(
                name: "IngredientCategory");

            migrationBuilder.DropTable(
                name: "IngredientMeasurement");

            migrationBuilder.DropTable(
                name: "Measurement");

            migrationBuilder.DropIndex(
                name: "IX_RecipeIngredient_MeasurementId",
                table: "RecipeIngredient");

            migrationBuilder.DropIndex(
                name: "IX_Ingredient_IngredientCategoryId",
                table: "Ingredient");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "RecipeIngredient");

            migrationBuilder.DropColumn(
                name: "MeasurementId",
                table: "RecipeIngredient");

            migrationBuilder.DropColumn(
                name: "IngredientCategoryId",
                table: "Ingredient");
        }
    }
}
