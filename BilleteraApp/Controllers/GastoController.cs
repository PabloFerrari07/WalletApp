using BilleteraApp.Dtos;
using BilleteraApp.Models;
using BilleteraApp.Services;
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
        private readonly IGastoService _gastoService;
        public GastoController( IGastoService gastoService) {
            _gastoService = gastoService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GastoDto>>> ObtenerGastos()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var gastos = await _gastoService.ObtenerGastosAsync(userId);
            return Ok(gastos);
        }
        [Authorize]
        [HttpPost("RegistrarGasto")]
        public async Task<ActionResult<SaldoDto>> RegistrarGasto(GastoDto dto)
        {

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var saldo = await _gastoService.RegistrarGastoAsync(userId, dto);
                return Ok(saldo);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize]
        [HttpPut("ActualizarGasto/{id}")]
        public async Task<IActionResult>ActualizarGasto(int id,GastoDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var actualizado = await _gastoService.ActualizarGastoAsync(userId, id, dto);
                if (!actualizado)
                    return NotFound("No se encontró el gasto.");

                return Ok("Gasto actualizado correctamente.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("EliminarGasto/{id}")]
        public async Task<IActionResult> EliminarGasto(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var eliminado = await _gastoService.EliminarGastoAsync(userId, id);

            if (!eliminado)
                return NotFound("No se encontró el gasto.");

            return Ok("Gasto eliminado correctamente.");
        }

    }
}
