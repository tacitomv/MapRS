using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Mapa.Migrations
{
    public partial class poistotags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
