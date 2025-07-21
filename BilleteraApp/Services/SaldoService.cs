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

            if (saldo == null) throw new Exception("No se encontró saldo.");

            saldo.MontoActual = dto.MontoActual;
            saldo.FechaActualizacion = DateTime.UtcNow;

            await _billeteraContext.SaveChangesAsync();

            return GenerarSaldoDto(saldo);
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

            _billeteraContext.HistorialSaldos.Add(new HistorialSaldo
            {
                UsuarioId = userId,
                Monto = dto.MontoActual,
                Fecha = DateTime.UtcNow,
                Tipo = "Ingreso"
            });

            await _billeteraContext.SaveChangesAsync();

            return GenerarSaldoDto(saldo);
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

            if (saldo == null) throw new Exception("No hay saldo registrado.");

            return GenerarSaldoDto(saldo);
        }


        private SaldoDto GenerarSaldoDto(Saldo saldo)
        {
            var dto = new SaldoDto
            {
                MontoActual = saldo.MontoActual,
                FechaActualizacion = saldo.FechaActualizacion
            };

            if (saldo.MontoActual < 500)
            {
                dto.SaldoBajo = true;
                dto.Alerta = "⚠️ ¡Atención! Tu saldo es muy bajo.";
            }

            return dto;
        }
    }
}
