using BilleteraApp.Dtos;
using BilleteraApp.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace BilleteraApp.Services
{
    public class AuthService : IAuthService
    {

        private readonly BilleteraContext _context;
        private readonly JwtService _jwtService;

        public AuthService(BilleteraContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;

        }

        public async Task<string> Login(LoginDto dto)
        {
            var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (usuario == null || !BCrypt.Net.BCrypt.Verify(dto.PasswordHash, usuario.PasswordHash))
            {
                return null; // Devuelve null = error
            }

            var token = _jwtService.GenerarToken(usuario);
            return token; // Devuelve el string
        }

        public async Task<UsuarioDto> RegistrarUsuario(RegisterDto dto)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);


            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash)
            };

            await _context.AddAsync(usuario);
            await _context.SaveChangesAsync();

            var usuarioDto = new UsuarioDto
            {
                Nombre = usuario.Nombre,
                Email = usuario.Email,

            };

            return usuarioDto;
        }
    }
}
