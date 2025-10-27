namespace BilleteraApp.Models
{
    public class Usuario
    {
        public int Id {  get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash {  get; set; }

        public Saldo Saldo { get; set; }
        public List<Gasto> Gastos { get; set; }
        public List<Categoria> Categorias { get; set; }
        public List<HistorialSaldo> HistorialSaldos { get; set; }

        public ICollection<Recomendacion> Recomendaciones { get; set; }

        public ICollection<GastoIA> GastosIA { get; set; }
    }
}
