using System.Text.Json;
using Newtonsoft.Json.Linq;

namespace CurrencyConversion;

public class ExchangeRateService
{
    private readonly HttpClient _httpClient;

    public ExchangeRateService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Dictionary<string, double>?> GetAllRates(string inputCurr)
    {        
        var resp = await _httpClient.GetAsync($"https://open.er-api.com/v6/latest/{inputCurr}");
        resp.EnsureSuccessStatusCode();

        var jsonStr = await resp.Content.ReadAsStringAsync();
        var jsonObj = JObject.Parse(jsonStr);
        return jsonObj["rates"] == null ? null : JsonSerializer.Deserialize<Dictionary<string, double>>(jsonObj["rates"].ToString());
    }
}