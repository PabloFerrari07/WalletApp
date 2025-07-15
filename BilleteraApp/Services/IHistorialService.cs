using BilleteraApp.Dtos;

namespace BilleteraApp.Services
{
    public interface IHistorialService
    {
        Task<IEnumerable<HistorialSaldoDto>> ObtenerHistorialAsync(int userId);
    }
}
