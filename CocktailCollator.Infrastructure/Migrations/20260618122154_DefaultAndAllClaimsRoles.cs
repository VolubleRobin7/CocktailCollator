using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CocktailCollator.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class DefaultAndAllClaimsRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "DefaultRole",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasEveryPermissionClaim",
                table: "AspNetRoles",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DefaultRole",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "HasEveryPermissionClaim",
                table: "AspNetRoles");
        }
    }
}
