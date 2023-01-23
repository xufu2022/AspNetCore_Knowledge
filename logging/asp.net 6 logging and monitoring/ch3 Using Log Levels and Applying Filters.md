# Using Log Levels and Applying Filters

## Intentions of Application Logging

DO
- Enable support and analysis
- Make it easily consumable
- Improve “fixability”
- Provide “complete-enough” information
- 
DON’T
- Clutter application code
- Impact performance
- Leak sensitive data
- Incur too much cost

## Log Levels

- Trace (never production)
- Debug (caution)
- Information
- Warning
- Error
- Critical (no disk space, db unavailable, etc)

```c#
//MapFallback(IEndpointRouteBuilder, Delegate)
//Adds a specialized RouteEndpoint to the IEndpointRouteBuilder that will match requests for non-file-names with the lowest possible priority.

app.MapFallback(() => Results.Redirect("/swagger"));
```
## Log Category

> Specified when creating ILogger

> Method 1: Inject ILogger<T> with T = class name

> Method 2: Inject ILoggerFactory and pass any string to CreateLogger

> Used in filters

## Log Filters

> Provider + Category + Minimum Level

> Key mechanism to control noise in logs
- Be careful in high-traffic environments

> Specify via configuration or code
- Configuration = any source (appsettings, environment variables, command line, etc.)

## Categories in Filters

Segments that applicable entries start with

Category Filter                     Applies To
CarvedRock.Domain.ProductLogic      CarvedRock.Domain.ProductLogic

CarvedRock.Domain                   CarvedRock.Domain.ProductLogic || CarvedRock.Domain.InventoryLogic

CarvedRock                          CarvedRock.Domain.ProductLogic || CarvedRock.Domain.InventoryLogic || CarvedRock.Api.Controllers.ProductController


same effects can be achieved by setting in both places
```json
//launch.json
            "env": {
                "ASPNETCORE_ENVIRONMENT": "Development"
                //"Logging__LogLevel__CarvedRock.Api.Controllers": "Debug"                
            },

//appsetting.json
    "Console": {
      "LogLevel": {
        "Default": "Information",
        "CarvedRock.Domain": "Debug",
        "Microsoft.AspNetCore": "Warning"
      },
      "FormatterName": "json",
      "FormatterOptions": {
        "SingleLine": true,
        "IncludeScopes": true,
        "TimestampFormat": "HH:mm:ss ",
        "UseUtcTimestamp": true,
        "JsonWriterOptions": {
            "Indented": true
        }
      }          
    },
    "Debug": {
      "LogLevel": {
        "Default": "Debug",
        "Microsoft.AspNetCore": "Information"
      }
    }
```

## Filtering by provider

Provider = destination to write log entries

Use two providers to see filters
- Console – format with JSON
- Debug – uses System.Diagnostics.Debug which writes to TraceListeners

More on providers in Log Destinations

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

- The "Default" and "Microsoft.AspNetCore" categories are specified.
- The "Microsoft.AspNetCore" category applies to all categories that start with "Microsoft.AspNetCore". For example, this setting applies to the  "Microsoft.AspNetCore.Routing.EndpointMiddleware" category.
- The "Microsoft.AspNetCore" category logs at log level Warning and higher.
- A specific log provider is not specified, so LogLevel applies to all the enabled logging providers except for the Windows EventLog.

```c#
var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
// specialFolder.localApplicationData: 
//  Window: c:\users\[username]\appdata\local
//  mac: /users/<username>/.local/share
var tracePath = Path.Join(path, $"Log_CarvedRock_{DateTime.Now.ToString("yyyyMMdd-HHmm")}.txt");        
Trace.Listeners.Add(new TextWriterTraceListener(System.IO.File.CreateText(tracePath)));
Trace.AutoFlush = true;	
```

## Apply filter in code

```c#
var builder = WebApplication.CreateBuilder(args);
builder.Logging.AddFilter("CarvedRock", LogLevel.Debug);
```