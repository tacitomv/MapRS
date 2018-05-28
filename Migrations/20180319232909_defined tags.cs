using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Mapa.Migrations
{
    public partial class definedtags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_POIs_Tags_TagId",
                table: "POIs");

            migrationBuilder.DropIndex(
                name: "IX_POIs_TagId",
                table: "POIs");

            migrationBuilder.DropColumn(
                name: "TagId",
                table: "POIs");

            migrationBuilder.CreateTable(
                name: "POITag",
                columns: table => new
                {
                    POIId = table.Column<int>(nullable: false),
                    TagId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_POITag", x => new { x.POIId, x.TagId });
                    table.ForeignKey(
                        name: "FK_POITag_POIs_POIId",
                        column: x => x.POIId,
                        principalTable: "POIs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_POITag_Tags_TagId",
                        column: x => x.TagId,
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_POITag_TagId",
                table: "POITag",
                column: "TagId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "POITag");

            migrationBuilder.AddColumn<int>(
                name: "TagId",
                table: "POIs",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_POIs_TagId",
                table: "POIs",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_POIs_Tags_TagId",
                table: "POIs",
                column: "TagId",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
