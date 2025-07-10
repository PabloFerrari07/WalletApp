using BilleteraApp.Dtos;
using BilleteraApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BilleteraApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GastoController : ControllerBase
    {
        private readonly BilleteraContext _billeteraContext;
        public GastoController(BilleteraContext billeteraContext) {
            _billeteraContext = billeteraContext;
        }

        [Authorize]
        [HttpPost("RegistrarGasto")]

        public async Task<ActionResult<SaldoDto>> RegistrarGasto(GastoDto dto)
        {
       
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

  
            var saldo = await _billeteraContext.Saldos.FirstOrDefaultAsync(s => s.UsuarioId == userId);

            if (saldo == null)
            {
                return BadRequest("No tienes saldo registrado");
            }

            if (saldo.MontoActual < dto.Monto)
            {
                return BadRequest("Fondos insuficientes");
            }

 
            var categoria = await _billeteraContext.Categorias
                .FirstOrDefaultAsync(c => c.Id == dto.CategoriaId && c.UsuarioId == userId);

            if (categoria == null)
            {
                return BadRequest("Categoría no encontrada o no pertenece a tu cuenta.");
            }

       
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

   
            var saldoActualizado = new SaldoDto
            {
                MontoActual = saldo.MontoActual,
                FechaActualizacion = saldo.FechaActualizacion
            };

            return Ok(saldoActualizado);
        }

    }
}
