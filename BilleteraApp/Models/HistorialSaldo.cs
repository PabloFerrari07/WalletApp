namespace BilleteraApp.Models
{
    public class HistorialSaldo
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Tipo { get; set; }  // 👈 ✅ El tipo: "Ingreso", "Gasto", etc.

        public Usuario Usuario { get; set; }
    }
}
