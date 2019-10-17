using Microsoft.EntityFrameworkCore.Migrations;

namespace Mapa.Migrations
{
    public partial class smthing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "traustit_MAPAGS_User");

            migrationBuilder.RenameTable(
                name: "Tags",
                newName: "Tags",
                newSchema: "traustit_MAPAGS_User");

            migrationBuilder.RenameTable(
                name: "POITag",
                newName: "POITag",
                newSchema: "traustit_MAPAGS_User");

            migrationBuilder.RenameTable(
                name: "POIs",
                newName: "POIs",
                newSchema: "traustit_MAPAGS_User");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                newName: "AspNetUserTokens",
                newSchema: "traustit_MAPAGS_User");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                newName: "AspNetUsers",
                newSchema: "traustit_MAPAGS_User");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                newName: "AspNetUserRoles",
                newSchema: "traustit_MAPAGS_User");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                newName: "AspNetUserLogins",
                newSchema: "traustit_MAPAGS_User");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                newName: "AspNetUserClaims",
                newSchema: "traustit_MAPAGS_User");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "AspNetRoles",
                newSchema: "traustit_MAPAGS_User");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                newName: "AspNetRoleClaims",
                newSchema: "traustit_MAPAGS_User");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "Tags",
                schema: "traustit_MAPAGS_User",
                newName: "Tags");

            migrationBuilder.RenameTable(
                name: "POITag",
                schema: "traustit_MAPAGS_User",
                newName: "POITag");

            migrationBuilder.RenameTable(
                name: "POIs",
                schema: "traustit_MAPAGS_User",
                newName: "POIs");

            migrationBuilder.RenameTable(
                name: "AspNetUserTokens",
                schema: "traustit_MAPAGS_User",
                newName: "AspNetUserTokens");

            migrationBuilder.RenameTable(
                name: "AspNetUsers",
                schema: "traustit_MAPAGS_User",
                newName: "AspNetUsers");

            migrationBuilder.RenameTable(
                name: "AspNetUserRoles",
                schema: "traustit_MAPAGS_User",
                newName: "AspNetUserRoles");

            migrationBuilder.RenameTable(
                name: "AspNetUserLogins",
                schema: "traustit_MAPAGS_User",
                newName: "AspNetUserLogins");

            migrationBuilder.RenameTable(
                name: "AspNetUserClaims",
                schema: "traustit_MAPAGS_User",
                newName: "AspNetUserClaims");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                schema: "traustit_MAPAGS_User",
                newName: "AspNetRoles");

            migrationBuilder.RenameTable(
                name: "AspNetRoleClaims",
                schema: "traustit_MAPAGS_User",
                newName: "AspNetRoleClaims");
        }
    }
}
