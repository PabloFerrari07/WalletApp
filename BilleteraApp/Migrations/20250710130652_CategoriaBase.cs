using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BilleteraApp.Migrations
{
    /// <inheritdoc />
    public partial class CategoriaBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoriasBase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoriasBase", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CategoriasBase",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Comida" },
                    { 2, "Transporte" },
                    { 3, "Ocio" },
                    { 4, "Servicios" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoriasBase");
        }
    }
}
