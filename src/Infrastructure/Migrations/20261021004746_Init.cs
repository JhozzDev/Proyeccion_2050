using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Proyeccion2050.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FilasPoblacion",
                columns: table => new
                {
                    Sexo = table.Column<int>(type: "INTEGER", nullable: false),
                    EdadMin = table.Column<int>(type: "INTEGER", nullable: false),
                    Anio = table.Column<int>(type: "INTEGER", nullable: false),
                    Poblacion = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FilasPoblacion", x => new { x.Sexo, x.EdadMin, x.Anio });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FilasPoblacion");
        }
    }
}
