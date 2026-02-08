namespace CurrencyConversion;

record ExchangeRateResponse(string currency, double value)
{
    public string Currency { get; set; }
    public double Value { get; set; }
}

public class ExchangeRateService
{
    private readonly HttpClient _httpClient;

    public ExchangeRate(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<List<
}