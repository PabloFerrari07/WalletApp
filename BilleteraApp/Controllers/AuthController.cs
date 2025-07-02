using BilleteraApp.Dtos;
using BilleteraApp.Models;
using BilleteraApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;




namespace BilleteraApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BilleteraContext _context;
        private readonly JwtService _jwtService;
        private readonly IAuthService _authService;
        public AuthController(BilleteraContext context, JwtService jwtService, IAuthService authService)
        {
            _context = context;
            _jwtService = jwtService;
            _authService = authService;

        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<RegisterDto>> RegistrarUsuario(RegisterDto dto)
        {
            if(await _context.Usuarios.AnyAsync(u => u.Email == dto.Email)) {
                return BadRequest("Usuario ya registrado");
            }

            var usuarioDto = await _authService.RegistrarUsuario(dto);

            return Ok(usuarioDto);

        }

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login(LoginDto dto)
        {
            var token = await _authService.Login(dto);

            if (token == null)
            {
                return BadRequest("Credenciales incorrectas.");
            }

            return Ok(new { token });
        }

    }
}
