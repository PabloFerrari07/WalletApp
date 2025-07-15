using BilleteraApp.Dtos;
using BilleteraApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BilleteraApp.Services
{
    public class SaldoService : ISaldoService
    {

        private readonly BilleteraContext _billeteraContext;
        public SaldoService(BilleteraContext billeteraContext)
        {
            _billeteraContext = billeteraContext;
        }


        public async Task<SaldoDto> ActualizarSaldoAsync(int userId, SaldoDto dto)
        {
            var saldo = await _billeteraContext.Saldos.FirstOrDefaultAsync(s => s.UsuarioId == userId);

            if (saldo == null) return null;

            saldo.MontoActual = dto.MontoActual;
            saldo.FechaActualizacion = DateTime.UtcNow;

            await _billeteraContext.SaveChangesAsync();

            return new SaldoDto
            {
                MontoActual = saldo.MontoActual,
                FechaActualizacion = saldo.FechaActualizacion
            };
        }

        public async Task<SaldoDto> CargarSaldoAsync(int userId, SaldoDto dto)
        {
            var saldo = await _billeteraContext.Saldos.FirstOrDefaultAsync(s => s.UsuarioId == userId);

            if (saldo == null)
            {
                saldo = new Saldo   
                {
                    UsuarioId = userId,
                    MontoActual = dto.MontoActual,
                    FechaActualizacion = DateTime.UtcNow
                };

                _billeteraContext.Saldos.Add(saldo);
            }
            else
            {
                saldo.MontoActual += dto.MontoActual;
                saldo.FechaActualizacion = DateTime.UtcNow;
            }

            // Registrar historial
            var historial = new HistorialSaldo
            {
                UsuarioId = userId,
                Monto = dto.MontoActual,
                Fecha = DateTime.UtcNow,
                Tipo = "Ingreso"
            };

            _billeteraContext.HistorialSaldos.Add(historial);

            await _billeteraContext.SaveChangesAsync();

            return new SaldoDto
            {
                MontoActual = saldo.MontoActual,
                FechaActualizacion = saldo.FechaActualizacion
            };
        }

        public async Task<bool> EliminarSaldoAsync(int userId)
        {
            var saldo = await _billeteraContext.Saldos.FirstOrDefaultAsync(s => s.UsuarioId == userId);

            if (saldo == null) return false;

            _billeteraContext.Saldos.Remove(saldo);
            await _billeteraContext.SaveChangesAsync();

            return true;
        }

        public async Task<SaldoDto> ObtenerSaldoAsync(int userId)
        {
            var saldo = await _billeteraContext.Saldos.FirstOrDefaultAsync(s => s.UsuarioId == userId);

            if (saldo == null) return null;

            return new SaldoDto
            {
                MontoActual = saldo.MontoActual,
                FechaActualizacion = saldo.FechaActualizacion
            };
        }
    }
}
