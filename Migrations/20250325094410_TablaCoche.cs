using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace proyecto_centauro.Migrations
{
    /// <inheritdoc />
    public partial class TablaCoche : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Marca = table.Column<string>(type: "TEXT", nullable: false),
                    Modelo = table.Column<string>(type: "TEXT", nullable: false),
                    Descripcion = table.Column<string>(type: "TEXT", nullable: false),
                    Patente = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo_coche = table.Column<string>(type: "TEXT", nullable: false),
                    Tipo_cambio = table.Column<string>(type: "TEXT", nullable: false),
                    Num_plazas = table.Column<int>(type: "INTEGER", nullable: false),
                    Num_maletas = table.Column<int>(type: "INTEGER", nullable: false),
                    Num_puertas = table.Column<int>(type: "INTEGER", nullable: false),
                    Posee_aire_acondicionado = table.Column<bool>(type: "INTEGER", nullable: false),
                    GrupoId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coches_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Coches_GrupoId",
                table: "Coches",
                column: "GrupoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Coches");
        }
    }
}
