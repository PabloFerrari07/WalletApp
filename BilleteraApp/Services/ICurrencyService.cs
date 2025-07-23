namespace BilleteraApp.Services
{
    public interface ICurrencyService
    {
        Task<decimal> ObtenerTipoCambioAsync(string baseCurrency, string targetCurrency);
    }
}
