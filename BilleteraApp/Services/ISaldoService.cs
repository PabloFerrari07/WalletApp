using BilleteraApp.Dtos;

namespace BilleteraApp.Services
{
    public interface ISaldoService
    {
        Task<SaldoDto> CargarSaldoAsync(int userId, SaldoDto dto);
        Task<SaldoDto> ObtenerSaldoAsync(int userId);
        Task<SaldoDto> ActualizarSaldoAsync(int userId, SaldoDto dto);
        Task<bool> EliminarSaldoAsync(int userId);
    }
}
