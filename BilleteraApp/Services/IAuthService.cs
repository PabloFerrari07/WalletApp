using BilleteraApp.Dtos;

namespace BilleteraApp.Services
{
    public interface IAuthService
    {
        Task<UsuarioDto> RegistrarUsuario(RegisterDto dto);
        Task<string> Login(LoginDto dto); // <-- CAMBIÁ a string
    }
}
