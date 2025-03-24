using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proyecto_centauro.Migrations
{
    /// <inheritdoc />
    public partial class TablaServicioAlquilerServicio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Servicios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nombre = table.Column<string>(type: "TEXT", nullable: false),
                    Precio = table.Column<decimal>(type: "TEXT", nullable: false),
                    PorcentajeDescuento = table.Column<decimal>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servicios", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AlquilerServicio");

            migrationBuilder.DropTable(
                name: "Servicios");
        }
    }
}
