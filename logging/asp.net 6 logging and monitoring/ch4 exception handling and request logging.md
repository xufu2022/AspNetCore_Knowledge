# Exception Handling and Request Logging

## Exception Handling Principles

- Provide an elegant user experience 
- Shield details from users – don’t help hackers!
- Enable support by providing ID’s and lookup capability
- Rely on your logs during local development
- Use global exception handling and try/catch only when needed

```c#
// Microsoft.AspNetCore.Mvc.ProblemDetails
public class ProblemDetails
{
public string? Type { get; set; } 
public string? Title { get; set; }
public int? Status { get; set; }
public string? Detail { get; set; }
public string? Instance { get; set; }
public IDictionary<string, object?> Extensions { get; }
}

```

 Middleware available in a NuGet package: **Hellang.Middleware.ProblemDetails**

- Update API error handling
- Use Hellang.Middleware.ProblemDetails
- Review handling and logging
- Provide some options to configure
- Middleware for critical error logging
- Update UI to consume ProblemDetails
- Deserialize response 
- Include in log entries

## Critical middleware

```c#
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (SqliteException sqlex)
        {
            if (sqlex.SqliteErrorCode == 551)
            {
                _logger.LogCritical(sqlex, "Fatal error occurred in database!!");
            }            
        }       
    }

builder.Services.AddProblemDetails(opts => 
{
    opts.IncludeExceptionDetails = (ctx, ex) => false;
    
    opts.OnBeforeWriteDetails = (ctx, dtls) => {
        if (dtls.Status == 500)
        {
            dtls.Detail = "An error occurred in our API. Use the trace id when contacting us.";
        }
    }; 
    opts.Rethrow<SqliteException>(); 
    opts.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
});

var app = builder.Build();

app.UseMiddleware<CriticalExceptionMiddleware>();
app.UseProblemDetails();

    <PackageReference Include="Hellang.Middleware.ProblemDetails" Version="6.3.0" />
```
```c#
            var response = await _apiClient.GetAsync($"Product?category={cat}");
            if (!response.IsSuccessStatusCode)
            {      
                 var fullPath = $"{_apiClient.BaseAddress}Product?category={cat}";                
                
                // trace id
                var details = await response.Content.ReadFromJsonAsync<ProblemDetails>() ??
                  new ProblemDetails();
                var traceId = details.Extensions["traceId"]?.ToString();

                _logger.LogWarning("API failure: {fullPath} Response: {response}, Trace: {trace}",
                  fullPath, (int) response.StatusCode, traceId);        

                throw new Exception("API call failed!");
            }
```

## Request Logging

> HTTP Logging
- Can log request / response body
- Uses logging providers: Informational from Microsoft.AspNetCore.HttpLogging
- Can impact performance
- Can leak sensitive data (be careful)
  
> W3C Logging
- Cannot log request or response body
- Writes to file, one line per request
- Can impact performance
- Can leak sensitive data (be careful)
- Also done by IIS, nginx, etc

**Use appsettings to disable HTTP logging**

```c#
builder.Services.AddHttpLogging(logging =>
{
    // https://bit.ly/aspnetcore6-httplogging
    logging.LoggingFields = HttpLoggingFields.All;
    logging.MediaTypeOptions.AddText("application/javascript");
    logging.RequestBodyLogLimit = 4096;
    logging.ResponseBodyLogLimit = 4096;    
});

var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
builder.Services.AddW3CLogging(opts => {
    // https://bit.ly/aspnetcore6-w3clogger
    opts.LoggingFields = W3CLoggingFields.All;
    opts.FileSizeLimit = 5 * 1024 * 1024;
    opts.RetainedFileCountLimit = 2;
    opts.FileName = "CarvedRock-W3C-UI";
    opts.LogDirectory = Path.Combine(path, "logs");
    opts.FlushInterval = TimeSpan.FromSeconds(2);    
});

builder.Services.AddHttpClient();

var app = builder.Build();
app.UseHttpLogging();
app.UseW3CLogging();
```

```json
"LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.AspNetCore.HttpLogging": "Information",
      "CarvedRock": "Debug"      
    },
    ...
"Console": {
      "FormatterName": "simple",
      "FormatterOptions": {
      "SingleLine": true,
      "IncludeScopes": true,
      "TimestampFormat": "HH:mm:ss ",
      "UseUtcTimestamp": true,
      "JsonWriterOptions": {
        "Indented": true
        }
      }
    }   
```
