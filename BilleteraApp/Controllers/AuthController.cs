using BilleteraApp.Dtos;
using BilleteraApp.Models;
using BilleteraApp.Services;
using FluentValidation;
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
        private IValidator<LoginDto> _validatorLogin;
        private IValidator<RegisterDto> _validatorRegister;

        public AuthController(BilleteraContext context, JwtService jwtService, IAuthService authService, IValidator<RegisterDto> validatorRegister, IValidator<LoginDto> validatorLogin)
        {
            _context = context;
            _jwtService = jwtService;
            _authService = authService;
            _validatorRegister = validatorRegister;
            _validatorLogin = validatorLogin;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<RegisterDto>> RegistrarUsuario(RegisterDto dto)
        {
            var validationResult = await _validatorRegister.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }


            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email)) {
                return BadRequest("Usuario ya registrado");
            }

            var usuarioDto = await _authService.RegistrarUsuario(dto);

            return Ok(usuarioDto);

        }

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login(LoginDto dto)
        {
            var validationResult = await _validatorLogin.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }
            var token = await _authService.Login(dto);

            if (token == null)
            {
                return BadRequest("Credenciales incorrectas.");
            }

            return Ok(new { token });
        }

    }
}
