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
    public class CategoriaController : ControllerBase
    {
        private readonly BilleteraContext _billeteraContext;
        public CategoriaController(BilleteraContext billeteraContext)
        {
            _billeteraContext = billeteraContext;   
        }

        [Authorize]
        [HttpGet("categorias")]
        public async Task<ActionResult<IEnumerable<CategoriaDto>>> ObtenerCategorias()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var categorias = await _billeteraContext.Categorias
                                                        .Where(c => c.UsuarioId == userId)
                                                        .Select(c => new CategoriaDto { Nombre = c.Nombre })
                                                        .ToListAsync();

            return Ok(categorias);
        }

        [Authorize]
        [HttpPost("agregarCategoria")]
        public async Task<ActionResult> CrearCategoria(CategoriaDto dto)
        {
            if(string.IsNullOrEmpty(dto.Nombre))
            {
                return BadRequest("el nombre de la categoria es obligatorio");
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            var existe = await _billeteraContext.Categorias.AnyAsync(c => c.UsuarioId ==userId && c.Nombre.ToLower() == dto.Nombre.ToLower());

            if (existe)
            {
                return BadRequest("Ya existe una categoria con ese nombre.");
            }

            var categoria = new Categoria
            {
                Nombre = dto.Nombre,
                UsuarioId = userId
            };

            _billeteraContext.Categorias.Add(categoria);
            await _billeteraContext.SaveChangesAsync();

            return Ok(new { mensaje = "Categoria creada correctamente." });
        }

    }
}
