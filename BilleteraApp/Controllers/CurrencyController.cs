using BilleteraApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BilleteraApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;

        public CurrencyController(ICurrencyService currencyService)
        {
            _currencyService = currencyService;
        }

        [HttpGet("ObtenerTipoCambio")]
        public async Task<ActionResult<decimal>> ObtenerTipoCambio(string baseCurrency, string toCurrency)
        {
            var tasa = await _currencyService.ObtenerTipoCambioAsync(baseCurrency, toCurrency);
            return Ok(new { tasa });
        }
    }
}
