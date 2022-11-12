# Rest : Representational State Transfer

Embrace async:

Use **IAsyncEnumberable** to avoid buffering and guarantee asynchronous iteration where possible

```csharp
[HttpGet ("products")]
public async IAsyncEnumerableIAsyncEnumerable<Product> <Product> ListAsyncListAsync()
{
    var products = _repository.GetProductsAsyncrepository.GetProductsAsync();
    await foreach (var product in products)
    {
        if ( product.IsOnSaleproduct.IsOnSale)
        {
            yield return product;
        }
    }
}
```

### Minimal API Responses

- IResult
- Object
- Raw String or JSON

## Apply Postel's law


```csharp
// using Ardalis.Result
public static Result<string> ToAtPrefixFormat(string twitterAlias)
{
    if (Regex.IsMatch(twitterAlias, "^@(\\w){ 1,15}$")) return twitterAlias; // already in correct format
    if (Regex.IsMatch(twitterAlias, "^(\\w){ 1,15}$")) return "@" + twitterAlias; // needs an @ prefix
    
    if (twitterAlias.StartsWith("http://twitter.com") ||
        twitterAlias.StartsWith("https://twitter.com")) // full URL – replace with just @username
    {
        // grab the last part of the URL
        string lastPart = twitterAlias.Split('/').Last();
        return "@" + lastPart;
    }

    return Result.Error("Input was not in a recognized format."); // fail if we make it here
}
```

## Localizing APIs

```csharp
builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(
    options =>
    {
        var supportedCultures = new List<CultureInfo>
            {
            new CultureInfo("en-US"),
            new CultureInfo("en-AU"),
            new CultureInfo("de-DE")
            };
    
        options.DefaultRequestCulture = new RequestCulture(culture: "en-US", uiCulture: "en-US");
        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
});
```
```csharp
// inject IStringLocalizer<Messages> in constructor
public override async Task<ActionResult<GreetingResponse>> HandleAsync([FromQuery]GreetingRequest request)
{
    var localizedString = _localizer["Greeting"];
    
    var response = new GreetingResponse { 
        GreetingFormatString = localizedString.Value,
        Greeting = String.Format(localizedString.Value, request.Name)
    };

    return Ok(response);
}
```

