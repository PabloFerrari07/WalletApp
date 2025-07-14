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
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GastoDto>>> ObtenerGastos()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var gastos = await _billeteraContext.Gastos
                                                .Where(g => g.UsuarioId == userId)
                                                .Include(g => g.Categoria)
                                                .OrderByDescending(g => g.Fecha)
                                                .ToListAsync();

            var gastosDto = gastos.Select(g => new GastoDto
            {
                Id = g.Id,
                Monto = g.Monto,
                Fecha = g.Fecha,
                Descripcion = g.Descripcion,
                CategoriaId = g.CategoriaId,
                CategoriaNombre = g.Categoria?.Nombre
            });

            return Ok(gastosDto);
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
        [Authorize]
        [HttpPut("ActualizarGasto/{id}")]
        public async Task<IActionResult>ActualizarGasto(int id,GastoDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var gasto = await _billeteraContext.Gastos
                .FirstOrDefaultAsync(g => g.Id == id && g.UsuarioId == userId);

            if (gasto == null)
            {
                return NotFound("No se encontró el gasto.");
            }

            var categoria = await _billeteraContext.Categorias.FirstOrDefaultAsync(c => c.Id == dto.CategoriaId && c.UsuarioId == userId);


            if(categoria == null)
            {
                return BadRequest("Categoria no encontrada o no pertenece a tu cuenta.");
            }


            gasto.Monto = dto.Monto;
            gasto.Descripcion = dto.Descripcion;
            gasto.CategoriaId = dto.CategoriaId;
            gasto.Fecha = DateTime.UtcNow;

            await _billeteraContext.SaveChangesAsync();

            return Ok("Gasto actualizado correctamente.");
        }

        [Authorize]
        [HttpDelete("EliminarGasto/{id}")]
        public async Task<IActionResult> EliminarGasto(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var gasto = await _billeteraContext.Gastos
                .FirstOrDefaultAsync(g => g.Id == id && g.UsuarioId == userId);

            if (gasto == null)
            {
                return NotFound("No se encontró el gasto.");
            }

            _billeteraContext.Gastos.Remove(gasto);

            await _billeteraContext.SaveChangesAsync();

            return Ok("Gasto eliminado correctamente.");
        }

    }
}
