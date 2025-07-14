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
        [HttpPost("CargarSaldo")]
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

        [Authorize]
        [HttpPut("ActualizarSaldo")]
        public async Task<ActionResult<SaldoDto>>ActualizarSaldo(SaldoDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var saldo = await _billeteraContext.Saldos.FirstOrDefaultAsync(s => s.UsuarioId == userId);

            if(saldo == null) {
                return NotFound("No se encontro saldo para actualizar");
            }


            saldo.MontoActual = dto.MontoActual;
            saldo.FechaActualizacion = DateTime.UtcNow;

            await _billeteraContext.SaveChangesAsync();

            var saldoDto = new SaldoDto
            {
                MontoActual = saldo.MontoActual,
                FechaActualizacion = saldo.FechaActualizacion
            };

            return Ok(saldoDto);
        }


        [Authorize]
        [HttpGet("ObtenerSaldo")]
        public async Task<ActionResult<SaldoDto>> ObtenerSaldo()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var saldo = await _billeteraContext.Saldos
                .FirstOrDefaultAsync(s => s.UsuarioId == userId);

            if (saldo == null)
            {
                return NotFound("No hay saldo registrado.");
            }

            var saldoDto = new SaldoDto
            {
                MontoActual = saldo.MontoActual,
                FechaActualizacion = saldo.FechaActualizacion
            };

            return Ok(saldoDto);
        }

        [Authorize]
        [HttpDelete("EliminarSaldo")]
        public async Task<IActionResult> EliminarSaldo()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var saldo = await _billeteraContext.Saldos.FirstOrDefaultAsync(s => s.UsuarioId == userId);

            if (saldo == null)
            {
                return NotFound("No se encontro saldo para actualizar");
            }

            _billeteraContext.Saldos.Remove(saldo);
            await _billeteraContext.SaveChangesAsync();

            return Ok("Saldo eliminado correctamente.");
        }



    }
}
