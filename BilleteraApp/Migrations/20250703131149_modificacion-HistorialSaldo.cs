using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BilleteraApp.Migrations
{
    /// <inheritdoc />
    public partial class modificacionHistorialSaldo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MontoAgregado",
                table: "HistorialSaldos",
                newName: "Monto");

            migrationBuilder.AddColumn<string>(
                name: "Tipo",
                table: "HistorialSaldos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tipo",
                table: "HistorialSaldos");

            migrationBuilder.RenameColumn(
                name: "Monto",
                table: "HistorialSaldos",
                newName: "MontoAgregado");
        }
    }
}
