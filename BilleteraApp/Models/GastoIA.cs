namespace BilleteraApp.Models
{
    public class GastoIA
    {
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public decimal Monto {  get; set; }

        public string Categoria {  get; set; }

        public DateTime Fecha { get; set; }

     
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
