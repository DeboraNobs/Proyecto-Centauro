using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proyecto_centauro.Migrations
{
    /// <inheritdoc />
    public partial class ServicioAlquilerprecio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlquilerServicio");

            migrationBuilder.CreateTable(
                name: "ServicioAlquiler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlquilerId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServicioId = table.Column<int>(type: "INTEGER", nullable: false),
                    Precio = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicioAlquiler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicioAlquiler_Alquileres_AlquilerId",
                        column: x => x.AlquilerId,
                        principalTable: "Alquileres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ServicioAlquiler_Servicios_ServicioId",
                        column: x => x.ServicioId,
                        principalTable: "Servicios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServicioAlquiler_AlquilerId",
                table: "ServicioAlquiler",
                column: "AlquilerId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicioAlquiler_ServicioId",
                table: "ServicioAlquiler",
                column: "ServicioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServicioAlquiler");

            migrationBuilder.CreateTable(
                name: "AlquilerServicio",
                columns: table => new
                {
                    AlquileresId = table.Column<int>(type: "INTEGER", nullable: false),
                    ServiciosId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AlquilerServicio", x => new { x.AlquileresId, x.ServiciosId });
                    table.ForeignKey(
                        name: "FK_AlquilerServicio_Alquileres_AlquileresId",
                        column: x => x.AlquileresId,
                        principalTable: "Alquileres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AlquilerServicio_Servicios_ServiciosId",
                        column: x => x.ServiciosId,
                        principalTable: "Servicios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AlquilerServicio_ServiciosId",
                table: "AlquilerServicio",
                column: "ServiciosId");
        }
    }
}
