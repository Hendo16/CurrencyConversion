using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CurrencyConversionTest;
public class ApiFactory : WebApplicationFactory<Program>
{
}
public class CurrencyConversionTest : IClassFixture<ApiFactory>
{
    private readonly HttpClient _client;
    
    public CurrencyConversionTest(ApiFactory factory)
    {
        _client = factory.CreateClient();
    }
    
    //Issue: Only testing a very specific use case; Would be good to generalise and ensure it would work for all currencies
    [Fact]
    public async Task AudToUsdTest()
    {
        var testSubmission = new CurrencySubmissionReq(5, "AUD", "USD");
        var response = await _client.PostAsJsonAsync("/ExchangeService", testSubmission);

        Assert.True(response.IsSuccessStatusCode);
        //Issue: Since currency conversion is always changing, the only way we can really test this is make sure the response object is mapped over successfully
        // by checking a known attribute, and also to make sure it isn't null.
        var currRespose = await response.Content
            .ReadFromJsonAsync<CurrencySubmissionResp>();
        Assert.NotNull(currRespose);
        Assert.Equal("AUD", currRespose.inputCurrency);

    }
}
