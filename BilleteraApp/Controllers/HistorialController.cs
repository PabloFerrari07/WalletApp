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
    public class HistorialController : ControllerBase
    {
        private readonly IHistorialService _historialService;
        public HistorialController(IHistorialService historialService) {
            _historialService = historialService;
        }

        [Authorize]
        [HttpGet("ObtenerHistorial")]
        public async Task<ActionResult<IEnumerable<HistorialSaldoDto>>> ObtenerHistorial()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var historial = await _historialService.ObtenerHistorialAsync(userId);

            return Ok(historial);
        }
    }
}
