using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proyecto_centauro.Migrations
{
    /// <inheritdoc />
    public partial class AgregarCamposAlquiler : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "HorarioDevolucion",
                table: "Alquileres",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HorarioRecogida",
                table: "Alquileres",
                type: "TEXT",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "LugarDevolucion",
                table: "Alquileres",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LugarRecogida",
                table: "Alquileres",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HorarioDevolucion",
                table: "Alquileres");

            migrationBuilder.DropColumn(
                name: "HorarioRecogida",
                table: "Alquileres");

            migrationBuilder.DropColumn(
                name: "LugarDevolucion",
                table: "Alquileres");

            migrationBuilder.DropColumn(
                name: "LugarRecogida",
                table: "Alquileres");
        }
    }
}
