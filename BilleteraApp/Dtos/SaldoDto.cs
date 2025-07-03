using BilleteraApp.Models;

namespace BilleteraApp.Dtos
{
    public class SaldoDto
    {
        public decimal MontoActual { get; set; }
        public DateTime FechaActualizacion { get; set; } // opcional
    }

}
