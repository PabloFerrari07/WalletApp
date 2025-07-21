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
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [Authorize]
        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> ObtenerCategorias()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var categorias = await _categoriaService.ObtenerCategorias(userId);
            return Ok(categorias);
        }

        [Authorize]
        [HttpPost("agregarCategoria")]
        public async Task<ActionResult> CrearCategoria(CategoriaDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _categoriaService.CrearCategoria(userId, dto);

            if (result.StartsWith("Ya") || result.StartsWith("El"))
                return BadRequest(result);

            return Ok(new { mensaje = result });
        }


        [Authorize]
        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarCategoria(int id, CategoriaDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _categoriaService.ActualizarCategoria(userId, id, dto);

            if (result.Contains("no encontrada") || result.Contains("vacío") || result.Contains("otra"))
                return BadRequest(result);

            return Ok(new { mensaje = result });
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> EliminarCategoria(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _categoriaService.EliminarCategoria(userId, id);

            if (result.Contains("No se puede") || result.Contains("no encontrada"))
                return BadRequest(result);

            return Ok(new { mensaje = result });
        }
    }
}
