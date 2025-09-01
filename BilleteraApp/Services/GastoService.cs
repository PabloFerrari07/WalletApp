using BilleteraApp.Dtos;
using BilleteraApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BilleteraApp.Services
{
    public class GastoService : IGastoService
    {
        private readonly BilleteraContext _billeteraContext;
        public GastoService(BilleteraContext billeteraContext)
        {
            _billeteraContext = billeteraContext;
        }
        public async Task<bool> ActualizarGastoAsync(int userId, int gastoId, GastoDto dto)
        {
            var gasto = await _billeteraContext.Gastos.FirstOrDefaultAsync(g => g.Id == gastoId && g.UsuarioId == userId);

            if (gasto == null)
                return false;

            var categoria = await _billeteraContext.Categorias.FirstOrDefaultAsync(c => c.Id == dto.CategoriaId && c.UsuarioId == userId);

            if (categoria == null)
                throw new InvalidOperationException("Categoría no encontrada o no pertenece a tu cuenta.");

            gasto.Monto = dto.Monto;
            gasto.Descripcion = dto.Descripcion;
            gasto.CategoriaId = dto.CategoriaId;
            gasto.Fecha = DateTime.UtcNow;

            await _billeteraContext.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<GastoEstadisticaDto>> ObtenerEstadisticasAsync(int userId, DateTime? desde = null, DateTime? hasta = null) {
            var query = _billeteraContext.Gastos
                                         .Where(g => g.UsuarioId == userId);

            if (desde.HasValue) query = query.Where(g => g.Fecha.Date >= desde.Value.Date);

            if(hasta.HasValue) query = query.Where(g=> g.Fecha.Date <= hasta.Value.Date);


            var resultado = await query.GroupBy(g => g.Fecha.Date).Select(g => new GastoEstadisticaDto
            {
                Fecha = g.Key,
                TotalGasto = g.Sum(x => x.Monto)
            })
                .OrderBy(e => e.Fecha)
                .ToListAsync();


            return resultado;
        }

        public async Task<bool> EliminarGastoAsync(int userId, int gastoId)
        {
            var gasto = await _billeteraContext.Gastos
                .FirstOrDefaultAsync(g => g.Id == gastoId && g.UsuarioId == userId);

            if (gasto == null)
                return false;

            // Buscar el saldo actual del usuario
            var saldo = await _billeteraContext.Saldos
                .FirstOrDefaultAsync(s => s.UsuarioId == userId);

            if (saldo != null)
            {
                // Sumar el monto del gasto al saldo actual
                saldo.MontoActual += gasto.Monto;
            }

            // Eliminar el gasto
            _billeteraContext.Gastos.Remove(gasto);
            await _billeteraContext.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<GastoDto>> ObtenerGastosAsync(int userId)
        {
            var gastos = await _billeteraContext.Gastos.Where(g => g.UsuarioId == userId)
                .Include(g => g.Categoria)
                .OrderByDescending(g => g.Fecha)
                .ToListAsync();

            return gastos.Select(g => new GastoDto
            {
                Id = g.Id,
                Monto = g.Monto,
                Fecha = g.Fecha,
                Descripcion = g.Descripcion,
                CategoriaId = g.CategoriaId,
                CategoriaNombre = g.Categoria?.Nombre
            });
        }

        public async Task<SaldoDto> RegistrarGastoAsync(int userId, GastoDto dto)
        {
            var saldo = await _billeteraContext.Saldos.FirstOrDefaultAsync(s => s.UsuarioId == userId);

            if (saldo == null)
                throw new InvalidOperationException("No tienes saldo registrado.");

            if (saldo.MontoActual < dto.Monto)
                throw new InvalidOperationException("Fondos insuficientes.");

            var categoria = await _billeteraContext.Categorias.FirstOrDefaultAsync(c => c.Id == dto.CategoriaId && c.UsuarioId == userId);

            if (categoria == null)
                throw new InvalidOperationException("Categoría no encontrada o no pertenece a tu cuenta.");

            saldo.MontoActual -= dto.Monto;
            saldo.FechaActualizacion = DateTime.UtcNow;

            var gasto = new Gasto
            {
                UsuarioId = userId,
                Monto = dto.Monto,
                Fecha = DateTime.UtcNow,
                Descripcion = dto.Descripcion,
                CategoriaId = dto.CategoriaId
            };

            _billeteraContext.Gastos.Add(gasto);

            var historial = new HistorialSaldo
            {
                UsuarioId = userId,
                Monto = -dto.Monto,
                Fecha = DateTime.UtcNow,
                Tipo = "Gasto"
            };

            _billeteraContext.HistorialSaldos.Add(historial);

            await _billeteraContext.SaveChangesAsync();

            return new SaldoDto
            {
                MontoActual = saldo.MontoActual,
                FechaActualizacion = saldo.FechaActualizacion
            };
        }
    }
}
