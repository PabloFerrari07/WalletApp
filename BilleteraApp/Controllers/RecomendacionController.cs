using BilleteraApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BilleteraApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecomendacionController : ControllerBase
    {
        private readonly IRecomendacionService _recomendacionService;

        public RecomendacionController(IRecomendacionService recomendacionService)
        {
            _recomendacionService = recomendacionService;
        }

        // 🔹 GET: api/Recomendaciones
        [Authorize]
        [HttpGet]
        public IActionResult GetRecomendaciones()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            var recomendaciones = _recomendacionService.ObtenerRecomendacionesActivas(usuarioId);
            return Ok(recomendaciones);
        }

        // 🔹 POST: api/Recomendaciones/Generar
        // (opcional, si querés forzar generar recomendaciones en cualquier momento)
        [Authorize]
        [HttpPost("Generar")]
        public IActionResult Generar()
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value!);
            _recomendacionService.GenerarRecomendaciones(usuarioId);
            return Ok(new { mensaje = "Recomendaciones generadas." });
        }

        // 🔹 PUT: api/Recomendaciones/MarcarLeida/5
        [Authorize]
        [HttpPut("MarcarLeida/{id}")]
        public IActionResult MarcarLeida(int id)
        {
            _recomendacionService.MarcarComoLeida(id);
            return Ok(new { mensaje = "Recomendación marcada como leída." });
        }
    }
}
