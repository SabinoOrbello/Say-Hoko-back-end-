using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Say_Hoko.Migrations
{
    public partial class AggiungiNuovaProprietaAOrdini : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Indirizzo",
                table: "Ordini",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Citta",
                table: "Ordini",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NumeroTelefono",
                table: "Ordini",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Ordini",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Indirizzo",
                table: "Ordini");

            migrationBuilder.DropColumn(
                name: "Citta",
                table: "Ordini");

            migrationBuilder.DropColumn(
                name: "NumeroTelefono",
                table: "Ordini");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Ordini");
        }
    }
}
