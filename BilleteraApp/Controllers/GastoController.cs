using BilleteraApp.Dtos;
using BilleteraApp.Models;
using BilleteraApp.Services;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;

namespace BilleteraApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GastoController : ControllerBase
    {
        private readonly IGastoService _gastoService;
        private readonly BilleteraContext _billeteraContext;
        private readonly IAService _iaService;

        private IValidator<GastoDto> _validator;
        public GastoController( IGastoService gastoService, BilleteraContext billeteraContext, IValidator<GastoDto> validator, IAService iaService)
        {
            _gastoService = gastoService;
            _billeteraContext = billeteraContext;
            _validator = validator;
            _iaService = iaService;
        }
        [Authorize]
        [HttpGet("PromediarEstadisticas")]
        public IActionResult GetEstadisticas()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var gastos = _billeteraContext.Gastos
                .Include(g => g.Categoria)
                .Where(g => g.UsuarioId == usuarioId)
                .ToList();

            var estadisticas = _gastoService.CalcularEstadisticas(gastos);
            return Ok(estadisticas);
        }

        [Authorize]
        [HttpGet("estadisticas-semana")]
        public async Task<IActionResult> ObtenerEstadisticasUltimos7Dias()
        {
            var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var estadisticas = await _gastoService.ObtenerEstadisticasAsync(userId);

            return Ok(estadisticas);
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
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

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
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

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
