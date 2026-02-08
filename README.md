Basic ASP.NET Core API that connects to the unauthenticated base tier of exchangerate-api 

# Building
Ensure you have Visual Studio/JetBrains Rider installed along with support for building ASP.NET Core projects. You will also need to have the .NET 9.0 SDK or greater installed.
Clone the repository, open the project and ensure that you have all of the dependencies installed before running.
Do note that you can run the app in either HTTP or HTTPS mode, which will change the port you access the API on.

| HTTP      | HTTPS      |
| ------------- | ------------- |
| 5214 | 7000 |

The application only has a single endpoint; 'EchangeService' that accepts a json body that follows this template
```
{
	"amount":5,
	"inputCurrency":"AUD",
	"outputCurrency":"USD"
}
```

You will recieve a similar JSON object in response

```
{
	"amount": 5,
	"inputCurrency": "AUD",
	"outputCurrency": "USD",
	"value": 3.5
}
```

# Issues
The project has several projects that should be taken into account.
- As mentioned above, the project only has a single endpoint. There's plenty of data provided by the API and plenty of possiblities that aren't being explored, such as comparing several currencies against the provided base.
- Error handling is very minimal, it only checks to make sure the list of exchanges rates are returned from the API as this is the most crucial data; This needs to be expanded for other points of failures.
- The requirements I recieved did specify that it should only allow AUD -> USD, *technically* this isn't being met as the value of inputCurrency is being passed directly to the API endpoint but it was such an easy fix I couldn't just ignore that!
- There's no options being passed to the endpoint to expand on the data being provided. For example, if you wanted the full amount of of the conversion that's currently not possible as we are rounding to 2 decimals.
- There's also no handling of rate limits on the free API itself so if you ran this multiple times it would probably crash - nor are we testing this. If there's any problem that's high priority, this would be it.
- We also aren't supporting providing your own API key - given that the base API can be expanded upon by simply signing up and providing your own, this seems like a good area for expansion.
