using BilleteraApp.Models;
using System.Text;
using System.Text.Json;

namespace BilleteraApp.Services
{
    public class IAService : IIAService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://smartwallet-ia.onrender.com";

        public IAService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> AnalizarGastosAsync(List<GastoIARequest> gastos)
        {
            var json = JsonSerializer.Serialize(gastos);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{BaseUrl}/analisis", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GenerarReporteAsync(List<GastoIARequest> gastos)
        {
            var json = JsonSerializer.Serialize(gastos);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{BaseUrl}/reporte", content);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
    }
