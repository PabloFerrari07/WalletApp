using BilleteraApp.Dtos;
using BilleteraApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BilleteraApp.Services
{
    public class CategoriaService : ICategoriaService
    {

        private readonly BilleteraContext _billeteraContext;

        public CategoriaService(BilleteraContext billeteraContext)
        {
            _billeteraContext = billeteraContext;
        }
        public async Task<string> ActualizarCategoria(int userId, int id, CategoriaDto dto)
        {
                var categoria = await _billeteraContext.Categorias
          .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == userId);

            if (categoria == null)
                return "Categoría no encontrada.";

            if (string.IsNullOrWhiteSpace(dto.Nombre))
                return "El nombre no puede estar vacío.";

            var existe = await _billeteraContext.Categorias
                .AnyAsync(c => c.UsuarioId == userId && c.Nombre.ToLower() == dto.Nombre.ToLower() && c.Id != id);

            if (existe)
                return "Ya tienes otra categoría con ese nombre.";

            categoria.Nombre = dto.Nombre;
            await _billeteraContext.SaveChangesAsync();

            return "Categoría actualizada correctamente.";
        }

        public async Task<string> CrearCategoria(int userId, CategoriaDto dto)
        {
            if (string.IsNullOrEmpty(dto.Nombre))
                return "El nombre de la categoría es obligatorio.";

            var existe = await _billeteraContext.Categorias
                .AnyAsync(c => c.UsuarioId == userId && c.Nombre.ToLower() == dto.Nombre.ToLower());

            if (existe)
                return "Ya existe una categoría con ese nombre.";

            var categoria = new Categoria
            {
                Nombre = dto.Nombre,
                UsuarioId = userId
            };

            _billeteraContext.Categorias.Add(categoria);
            await _billeteraContext.SaveChangesAsync();

            return "Categoría creada correctamente.";
        }

        public async Task<string> EliminarCategoria(int userId, int id)
        {
            var categoria = await _billeteraContext.Categorias
      .FirstOrDefaultAsync(c => c.Id == id && c.UsuarioId == userId);

            if (categoria == null)
                return "Categoría no encontrada.";

            var tieneGastos = await _billeteraContext.Gastos.AnyAsync(g => g.CategoriaId == id);

            if (tieneGastos)
                return "No se puede eliminar: existen gastos asociados.";

            _billeteraContext.Categorias.Remove(categoria);
            await _billeteraContext.SaveChangesAsync();

            return "Categoría eliminada correctamente.";
        }

        public async Task<IEnumerable<CategoriaDto>> ObtenerCategorias(int userId)
        {
            return await _billeteraContext.Categorias
                   .Where(c => c.UsuarioId == userId)
                   .Select(c => new CategoriaDto { Id = c.Id, Nombre = c.Nombre })
                   .ToListAsync();
        }
    }
}
