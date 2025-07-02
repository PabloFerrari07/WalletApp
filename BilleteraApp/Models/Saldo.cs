namespace BilleteraApp.Models
{
    public class Saldo
    {
        public int Id { get; set; }
        public Decimal MontoActual { get; set; }
        public DateTime FechaActualizacion {  get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
