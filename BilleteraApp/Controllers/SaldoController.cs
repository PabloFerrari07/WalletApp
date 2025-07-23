using BilleteraApp.Dtos;
using BilleteraApp.Models;
using BilleteraApp.Services;
using FluentValidation;
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

        private readonly ISaldoService _saldoService;
        private IValidator<SaldoDto> _validator;
        public SaldoController( ISaldoService saldoService,IValidator<SaldoDto> validator)
        {
            _saldoService = saldoService;
            _validator = validator;
        }

        [Authorize]
        [HttpPost("CargarSaldo")]
        public async Task<ActionResult<SaldoDto>> CargarSaldo(SaldoDto dto)
        {
            var validationResult = await _validator.ValidateAsync(dto);

            if (!validationResult.IsValid) {
                return BadRequest(validationResult.Errors);
            }
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _saldoService.CargarSaldoAsync(userId, dto);
            return Ok(result);
        }

        [Authorize]
        [HttpPut("ActualizarSaldo")]
        public async Task<ActionResult<SaldoDto>>ActualizarSaldo(SaldoDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _saldoService.ActualizarSaldoAsync(userId, dto);
            if (result == null) return NotFound("No se encontró saldo para actualizar");
            return Ok(result);
        }


        [Authorize]
        [HttpGet("ObtenerSaldo")]
        public async Task<ActionResult<SaldoDto>> ObtenerSaldo()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _saldoService.ObtenerSaldoAsync(userId);
            if (result == null) return NotFound("No hay saldo registrado");
            return Ok(result);
        }

        [Authorize]
        [HttpDelete("EliminarSaldo")]
        public async Task<IActionResult> EliminarSaldo()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var eliminado = await _saldoService.EliminarSaldoAsync(userId);
            if (!eliminado) return NotFound("No se encontró saldo para eliminar");
            return Ok("Saldo eliminado correctamente");
        }


        [Authorize]
        [HttpGet("ObtenerResumenMultiMoneda")]
        public async Task<ActionResult<SaldoResumenDto>> ObtenerResumenMultiMoneda([FromQuery] string toCurrency)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var resumen = await _saldoService.ObtenerResumenMultiMonedaAsync(userId, toCurrency);

            return Ok(resumen);
        }



    }
}
