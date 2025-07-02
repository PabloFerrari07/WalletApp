namespace BilleteraApp.Models
{
    public class Gasto
    {
        public int Id { get; set; }
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }

        // FK a Usuario
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }

        // FK a Categoria
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}
