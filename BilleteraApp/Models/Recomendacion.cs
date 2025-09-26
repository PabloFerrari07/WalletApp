namespace BilleteraApp.Models
{
    public class Recomendacion
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public string Mensaje { get; set; }
        public DateTime Fecha { get; set; }
        public bool Activa { get; set; } = true; // Para marcar si ya fue mostrada/leída
    }
}
