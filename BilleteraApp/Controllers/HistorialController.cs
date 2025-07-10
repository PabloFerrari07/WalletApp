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
    public class HistorialController : ControllerBase
    {
        private readonly BilleteraContext _billeteraContext;
        public HistorialController(BilleteraContext billeteraContext) {
            _billeteraContext = billeteraContext;
        }

        [Authorize]
        [HttpGet("ObtenerHistorial")]
        public async Task<ActionResult<IEnumerable<HistorialSaldoDto>>> ObtenerHistorial()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var historial = await _billeteraContext.HistorialSaldos
                .Where(h => h.UsuarioId == userId)
                .OrderByDescending(h => h.Fecha).ToListAsync();

            var historialDto = historial.Select(h => new HistorialSaldoDto
            {
                Monto = h.Monto,
                Fecha = h.Fecha,
                Tipo = h.Tipo
            });

            return Ok(historialDto);
        }
    }
}
