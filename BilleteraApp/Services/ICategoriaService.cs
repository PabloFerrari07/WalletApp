using BilleteraApp.Dtos;

namespace BilleteraApp.Services
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaDto>> ObtenerCategorias(int userId);
        Task<string> CrearCategoria(int userId, CategoriaDto dto);
        Task<string> ActualizarCategoria(int userId, int id, CategoriaDto dto);
        Task<string> EliminarCategoria(int userId, int id);
    }
}
