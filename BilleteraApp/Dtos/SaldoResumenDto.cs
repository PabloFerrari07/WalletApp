namespace BilleteraApp.Services
{
    public class SaldoResumenDto
    {
        public decimal Saldo { get; set; }
        public string MonedaLocal { get; set; } = "ARS";
        public string ConvertidoA { get; set; }
        public decimal Tasa { get; set; }
        public decimal SaldoConvertido { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
