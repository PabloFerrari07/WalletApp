using BilleteraApp.Models;

namespace BilleteraApp.Dtos
{
    public class SaldoDto
    {
        public int UsuarioId { get; set; }
        public decimal MontoActual { get; set; }
        public DateTime FechaActualizacion { get; set; } // opcional

        public bool SaldoBajo { get; set; }
        public string? Alerta { get; set; }
    }

}
