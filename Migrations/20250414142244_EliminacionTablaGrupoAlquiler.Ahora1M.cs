using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proyecto_centauro.Migrations
{
    /// <inheritdoc />
    public partial class EliminacionTablaGrupoAlquilerAhora1M : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GrupoAlquiler");

            migrationBuilder.AddColumn<int>(
                name: "GrupoId",
                table: "Alquileres",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Alquileres_GrupoId",
                table: "Alquileres",
                column: "GrupoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Alquileres_Grupos_GrupoId",
                table: "Alquileres",
                column: "GrupoId",
                principalTable: "Grupos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Alquileres_Grupos_GrupoId",
                table: "Alquileres");

            migrationBuilder.DropIndex(
                name: "IX_Alquileres_GrupoId",
                table: "Alquileres");

            migrationBuilder.DropColumn(
                name: "GrupoId",
                table: "Alquileres");

            migrationBuilder.CreateTable(
                name: "GrupoAlquiler",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AlquilerId = table.Column<int>(type: "INTEGER", nullable: false),
                    GrupoId = table.Column<int>(type: "INTEGER", nullable: false),
                    Precio = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrupoAlquiler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GrupoAlquiler_Alquileres_AlquilerId",
                        column: x => x.AlquilerId,
                        principalTable: "Alquileres",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GrupoAlquiler_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GrupoAlquiler_AlquilerId",
                table: "GrupoAlquiler",
                column: "AlquilerId");

            migrationBuilder.CreateIndex(
                name: "IX_GrupoAlquiler_GrupoId",
                table: "GrupoAlquiler",
                column: "GrupoId");
        }
    }
}
