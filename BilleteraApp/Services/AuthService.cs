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
            // Verifica si ya existe
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
            {
                throw new Exception("El usuario ya existe con ese email.");
            }

            // Genera el hash
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.PasswordHash);

            // Crea el usuario
            var usuario = new Usuario
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                PasswordHash = passwordHash
            };

            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();

            // ✅ Clona CategoriasBase para este usuario
            var categoriasBase = await _context.CategoriasBase.ToListAsync();

            var categoriasUsuario = categoriasBase.Select(baseCat => new Categoria
            {
                Nombre = baseCat.Nombre,
                UsuarioId = usuario.Id
            }).ToList();

            await _context.Categorias.AddRangeAsync(categoriasUsuario);
            await _context.SaveChangesAsync();

            // Devuelve DTO
            var usuarioDto = new UsuarioDto
            {
                Nombre = usuario.Nombre,
                Email = usuario.Email
            };

            return usuarioDto;
        }

    }
}
