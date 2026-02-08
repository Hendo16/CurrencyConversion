//Issue: None of this is in a native class; Great for small boilerplate projects like this but needs to be expanded on
using CurrencyConversion;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
} 
app.UseExceptionHandler("/error");
app.UseHttpsRedirection();

//Issue: Generic error handler. Would be better to expand on this and have dynamic responses depending on what failed.
app.Map("/error", (HttpContext context) =>
{
    var exception = context.Features
        .Get<IExceptionHandlerFeature>()?.Error;

    var problemDetails = new ProblemDetails
    {
        Status = StatusCodes.Status500InternalServerError,
        Title = "Error occurred!",
        Detail = exception?.Message ?? "Please check the logs"
    };

    return Results.Problem(problemDetails);
});

app.MapPost("/ExchangeService", async (CurrencySubmissionReq currSubmit) =>
{
    //Figured it'd look cleaner to map the json data to variables like this at the top
    var amount = currSubmit.amount;
    var inputCurrency = currSubmit.inputCurrency;
    var outputCurrency = currSubmit.outputCurrency;
    
    //Issue: Creating a new HttpClient instance someone makes a new request. Terrible! API should be have access to a single, globally available instance.
    var exchangeService = new ExchangeRateService(new HttpClient());
    var rates = await exchangeService.GetAllRates(inputCurrency);
    //Issue: We're just generally checking if the rates response is empty but we aren't validating the requested currency is available. Plus, it'd be better if we had customized exceptions
    //instead of a generic one.
    if (rates == null)
    {
        throw new InvalidOperationException("Rates were empty!");
    }
    
    var usdRate = rates[outputCurrency];
    var convertedValue = Math.Round(amount * usdRate, 2);
    return new CurrencySubmissionResp(amount, inputCurrency, outputCurrency, convertedValue);
}).WithName("GetExchangeService");

app.Run();

//Issue: Would be better to have a more flexible request/response system as this could get a bit tedious as the scope grows, but it works for this tiny use case.
public record CurrencySubmissionReq(double amount, string inputCurrency, string outputCurrency);
public record CurrencySubmissionResp(double amount, string inputCurrency, string outputCurrency, double value);
//Include this classifier so xUnit is able to spin up an instance of this class for testing
public partial class Program
{
}