using BilleteraApp.Models;

namespace BilleteraApp.Services
{
    public interface IIAService
    {
        Task<string> AnalizarGastosAsync(List<GastoIARequest> gastos);
        Task<string> GenerarReporteAsync(List<GastoIARequest> gastos);
    }
}
