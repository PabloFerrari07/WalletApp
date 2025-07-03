using BilleteraApp.Dtos;
using BilleteraApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BilleteraApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaldoController : ControllerBase
    {
        private readonly BilleteraContext _billeteraContext;

        public SaldoController(BilleteraContext billeteraContext)
        {
            _billeteraContext = billeteraContext;
        }

        [Authorize]
        [HttpPost]
        [Route("CargarSaldo")]
        public async Task<ActionResult<SaldoDto>> CargarSaldo(SaldoDto dto)
        {
            // ✅ Sacar el ID del token (claim)
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized("Token inválido: no tiene ID de usuario.");
            }

            var userId = int.Parse(userIdClaim);

            // 🔍 Buscar saldo existente
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

            var saldoActualizado = new SaldoDto
            {
                MontoActual = saldo.MontoActual,
                FechaActualizacion = saldo.FechaActualizacion
            };

            return Ok(saldoActualizado);
        }


        /*
        [HttpGet]
        [Route("ObtenerSaldo")]

        public async Task<IEnumerable<SaldoDto>> ObtenerSaldo() => await _billeteraContext.Saldos.Select
            (b => new SaldoDto { FechaActualizacion=b.FechaActualizacion, MontoActual=b.MontoActual ,UsuarioId = b.UsuarioId}).ToListAsync();

        */
    }
}
