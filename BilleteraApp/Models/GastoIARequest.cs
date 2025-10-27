namespace BilleteraApp.Models
{
    public class GastoIARequest
    {
        public string Descripcion { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
    }
}
