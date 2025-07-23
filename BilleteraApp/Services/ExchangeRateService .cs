using System.Text.Json;

namespace BilleteraApp.Services
{
    public class ExchangeRateService : ICurrencyService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "4fe94f110c7ab6d2820dc5ba";

        public ExchangeRateService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> ObtenerTipoCambioAsync(string baseCurrency, string targetCurrency)
        {
            var url = $"https://v6.exchangerate-api.com/v6/{_apiKey}/latest/{baseCurrency}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();


            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;


            if (root.GetProperty("result").GetString() != "success")
                throw new Exception("Error al obtener tipo de cambio");

            var rates = root.GetProperty("conversion_rates");
            return rates.GetProperty(targetCurrency).GetDecimal();
        }

    }
}
