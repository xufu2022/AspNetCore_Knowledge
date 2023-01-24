# Log Destinations

## Default Providers

-   Set up by default:
    - Console
    - Debug
    - EventSource (for dotnet-trace)
    - EventLog (Windows only)
-   Can be “cleared” and added individually

### Demo

-   Using code to manage providers
-   See defaults in action
-   Clear providers, add JsonConsole
    - No debug entries anymore
-   Add Debug
    - Config in app settings


```c#
builder.Logging.ClearProviders();
// builder.Logging.AddJsonConsole();
// builder.Logging.AddDebug();
//builder.Services.AddApplicationInsightsTelemetry();
// builder.Host.UseSerilog((context, loggerConfig) => {
//     loggerConfig
//     .WriteTo.Console()
//     .Enrich.WithExceptionDetails()
//     .WriteTo.Seq("http://localhost:5341");
// });
NLog.LogManager.Setup().LoadConfigurationFromFile();
builder.Host.UseNLog();

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
```

## Semantic (Structured) Logging
-   State
    - Message: Message template with replacements
    - Template parameter names and their values
    - {OriginalFormat}: message template string
-   Scopes
    - Individual key-value pair sets
-   All of this should be easily visible and searchable

