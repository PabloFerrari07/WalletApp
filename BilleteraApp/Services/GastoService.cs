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

        public async Task<IEnumerable<GastoEstadisticaDto>> ObtenerEstadisticasAsync(int userId, DateTime? desde = null, DateTime? hasta = null)
        {
            var hoy = DateTime.UtcNow.Date;
            var hace7Dias = hoy.AddDays(-6); // Incluye hoy y los 6 días anteriores

            // Traer los gastos del usuario en los últimos 7 días
            var gastos = await _billeteraContext.Gastos
                .Where(g => g.UsuarioId == userId && g.Fecha.Date >= hace7Dias && g.Fecha.Date <= hoy)
                .GroupBy(g => g.Fecha.Date)
                .Select(g => new GastoEstadisticaDto
                {
                    Fecha = g.Key,
                    TotalGasto = g.Sum(x => x.Monto)
                })
                .ToListAsync();

            // Rellenar días faltantes con 0
            var resultado = Enumerable.Range(0, 7)
                .Select(i => hace7Dias.AddDays(i))
                .Select(dia => new GastoEstadisticaDto
                {
                    Fecha = dia,
                    TotalGasto = gastos.FirstOrDefault(g => g.Fecha == dia)?.TotalGasto ?? 0
                })
                .OrderBy(r => r.Fecha)
                .ToList();

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


        public GastosEstadisticasDto CalcularEstadisticas(List<Gasto> gastos)
        {
            if (gastos == null || !gastos.Any())
            {
                return new GastosEstadisticasDto
                {
                    TotalGastado = 0,
                    Promedio = 0,
                    CategoriaMasGastada = "Sin datos"
                };
            }

            var total = gastos.Sum(g => g.Monto);
            var promedio = gastos.Average(g => g.Monto);

            var categoriaMasGastada = gastos
                .GroupBy(g => g.Categoria.Nombre) // usamos el nombre de la categoría
                .Select(g => new { Categoria = g.Key, Total = g.Sum(x => x.Monto) })
                .OrderByDescending(g => g.Total)
                .First().Categoria;

            return new GastosEstadisticasDto
            {
                TotalGastado = total,
                Promedio = promedio,
                CategoriaMasGastada = categoriaMasGastada
            };
        }
    }
}
