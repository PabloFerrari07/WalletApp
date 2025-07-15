using BilleteraApp.Dtos;
using BilleteraApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BilleteraApp.Services
{
    public class HistorialService : IHistorialService
    {

        private readonly BilleteraContext _billeteraContext;

        public HistorialService(BilleteraContext billeteraContext)
        {
            _billeteraContext = billeteraContext;
        }
        public async Task<IEnumerable<HistorialSaldoDto>> ObtenerHistorialAsync(int userId)
        {
            var historial = await _billeteraContext.HistorialSaldos
              .Where(h => h.UsuarioId == userId)
              .OrderByDescending(h => h.Fecha)
              .ToListAsync();

                    return historial.Select(h => new HistorialSaldoDto
                    {
                        Monto = h.Monto,
                        Fecha = h.Fecha,
                        Tipo = h.Tipo
                    });
        }
    }
}
