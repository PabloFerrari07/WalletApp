using BilleteraApp.Models;
using BilleteraApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BilleteraApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly BilleteraContext _context;
        private readonly JwtService _jwtService;

        public AuthController(BilleteraContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;

        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<>> RegistrarUsuario()
        {

        }

    }
}
