# Getting Started with Logging

```c#
//Within Program.cs (our code): 

var builder = WebApplication.CreateBuilder(args);

//Within WebApplication.CreateBuilder (ASP.NET Core source):
.ConfigureLogging((hostingContext, loggingBuilder) =>
{
loggingBuilder.Configure(options =>  //tracking options
{
    options.ActivityTrackingOptions = ActivityTrackingOptions.SpanId
    | ActivityTrackingOptions.TraceId
    | ActivityTrackingOptions.ParentId;
    });

loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
loggingBuilder.AddConsole();
loggingBuilder.AddDebug();
loggingBuilder.AddEventSourceLogger();  // .net trace tools
})

```

> Default Logging Setup

• Activity tracking
• Configuration
• Default providers

Docs: https://bit.ly/aspnet6-logging-docs

```c#
//no dollar sign here
_logger.LogInformation("Getting products in API for {category}", category);
```

