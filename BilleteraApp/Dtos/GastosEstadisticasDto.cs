namespace BilleteraApp.Dtos
{
    public class GastosEstadisticasDto
    {
        public decimal TotalGastado { get; set; }
        public decimal Promedio { get; set; }
        public string CategoriaMasGastada { get; set; } = string.Empty;

        public List<GastoPorCategoriaDto> GastosPorCategoria { get; set; }
    }
}
