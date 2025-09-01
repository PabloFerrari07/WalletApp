using BilleteraApp.Dtos;

namespace BilleteraApp.Services
{
    public interface IGastoService
    {
        Task<IEnumerable<GastoDto>> ObtenerGastosAsync(int userId);
        Task<SaldoDto> RegistrarGastoAsync(int userId, GastoDto dto);
        Task<bool> ActualizarGastoAsync(int userId, int gastoId, GastoDto dto);
        Task<bool> EliminarGastoAsync(int userId, int gastoId);

        Task<IEnumerable<GastoEstadisticaDto>> ObtenerEstadisticasAsync(int userId, DateTime? desde = null, DateTime? hasta = null);
    }
}
