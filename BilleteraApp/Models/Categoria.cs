namespace BilleteraApp.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // FK a Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        public List<Gasto> Gastos { get; set; }
    }
}
