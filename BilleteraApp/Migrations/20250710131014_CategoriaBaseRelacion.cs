using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BilleteraApp.Migrations
{
    /// <inheritdoc />
    public partial class CategoriaBaseRelacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoriaBaseId",
                table: "Categorias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categorias_CategoriaBaseId",
                table: "Categorias",
                column: "CategoriaBaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categorias_CategoriasBase_CategoriaBaseId",
                table: "Categorias",
                column: "CategoriaBaseId",
                principalTable: "CategoriasBase",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categorias_CategoriasBase_CategoriaBaseId",
                table: "Categorias");

            migrationBuilder.DropIndex(
                name: "IX_Categorias_CategoriaBaseId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "CategoriaBaseId",
                table: "Categorias");
        }
    }
}
