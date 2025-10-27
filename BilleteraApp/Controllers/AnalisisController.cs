using BilleteraApp.Models;
using BilleteraApp.Services;
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
    public class AnalisisController : ControllerBase
    {
        private readonly BilleteraContext _context;
        private readonly IIAService _iaService;

        public AnalisisController(BilleteraContext context, IIAService iaService)
        {
            _context = context;
            _iaService = iaService;
        }

        [Authorize]
        [HttpGet("analisis")]
        public async Task<IActionResult> AnalizarGastos()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            int usuarioId;
            if (!int.TryParse(userId, out usuarioId))
                return Unauthorized();

            var gastos = await _context.Gastos
              .Where(g => g.UsuarioId == usuarioId)
              .Select(g => new GastoIARequest
              {
                  Descripcion = g.Descripcion,
                  Monto = g.Monto,
                  Categoria = g.Categoria.Nombre,
                  Fecha = g.Fecha
              })
              .ToListAsync();

            if (gastos.Count == 0)
                return Ok(new { mensaje = "No hay gastos registrados para analizar." });

            var resultado = await _iaService.AnalizarGastosAsync(gastos);
            return Ok(JsonDocument.Parse(resultado).RootElement);
        }

        [Authorize]
        [HttpGet("reporte")]
        public async Task<IActionResult> GenerarReporte()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userId == null)
                return Unauthorized();

            int usuarioId;
            if (!int.TryParse(userId, out usuarioId))
                return Unauthorized();
            var gastos = await _context.Gastos
              .Where(g => g.UsuarioId == usuarioId)
              .Select(g => new GastoIARequest
              {
                  Descripcion = g.Descripcion,
                  Monto = g.Monto,
                  Categoria = g.Categoria.Nombre,
                  Fecha = g.Fecha
              })
              .ToListAsync();

            if (gastos.Count == 0)
                return Ok(new { mensaje = "No hay gastos registrados para generar reporte." });

            var resultado = await _iaService.GenerarReporteAsync(gastos);
            return Ok(JsonDocument.Parse(resultado).RootElement);
        }
    }
}
