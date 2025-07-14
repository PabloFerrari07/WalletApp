namespace BilleteraApp.Dtos
{
    public class GastoDto
    {
        public int Id { get; set; } // Para el PUT y DELETE
        public decimal Monto { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public int CategoriaId { get; set; }
        public string CategoriaNombre { get; set; } // opcional, solo lectura
    }
}
