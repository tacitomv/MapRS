using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mapa.Migrations
{
    public partial class camposextras : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NomeContato",
                table: "POIs",
                newName: "ContactName");

            migrationBuilder.AlterColumn<double>(
                name: "Longitude",
                table: "POIs",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "POIs",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "CNPJ",
                table: "POIs",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FoundationDate",
                table: "POIs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CNPJ",
                table: "POIs");

            migrationBuilder.DropColumn(
                name: "FoundationDate",
                table: "POIs");

            migrationBuilder.RenameColumn(
                name: "ContactName",
                table: "POIs",
                newName: "NomeContato");

            migrationBuilder.AlterColumn<int>(
                name: "Longitude",
                table: "POIs",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Latitude",
                table: "POIs",
                nullable: false,
                oldClrType: typeof(double));
        }
    }
}
