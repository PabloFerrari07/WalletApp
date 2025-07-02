namespace BilleteraApp.Models
{
    public class HistorialSaldo
    {
        public int Id { get; set; }
        public decimal MontoAgregado { get; set; }
        public DateTime Fecha { get; set; }

        // FK a Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
