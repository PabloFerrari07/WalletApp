using BilleteraApp.Models;

namespace BilleteraApp.Services
{
    public interface IRecomendacionService
    {
        void GenerarRecomendaciones(int usuarioId);
        IEnumerable<Recomendacion> ObtenerRecomendacionesActivas(int usuarioId);
        void MarcarComoLeida(int recomendacionId);
    }
}
