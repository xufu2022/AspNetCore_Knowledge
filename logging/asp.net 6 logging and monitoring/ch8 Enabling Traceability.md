# Enabling Traceability

-   Activity tracking in ASP.NET Core
    - Log field summary
    - W3C and Hierarchical
-   Analyze transactions
    - Application Insights
-   Distributed computing and microservices
    - OpenTelemetry
    - Creating events and activities
    - Viewing transactions

## Defining Traceability

-   Trace or correlate a page / screen from browser to logs
-   Trace flow of activity or transaction
    - Could cross process, machine, or time boundaries
    - Might be user-initiated
-   ASP.NET Core has some built-in fields
-   Leverages System.Diagnostics.Activity and implements W3C trace context
-   Fields show up in logs – we can use them to do tracing

### Activity Tracking Log Fields

 | **Field**    | **Description** | **Example** |
|:-------------------|------------|------------|
| ActionId                              |Identifier for the action / route / page                               | de9ab21b-279b-42c6-b93e-b0d377c49f8e |
| ActionName                            |Name of the action / route / page                                      | CarvedRock.Api.Controllers.ProductController.Get (CarvedRock.Api) /Listing|
| ConnectionID                          |Can be shared across multiple navigations; can change within session.  | 0HMFC25O20STO|
| RequestID(HttpContext.TraceIdentifer) |Combination of RequestID and a sequence number for a request within a Connection | 0HMFC25O20STO:00000007|
| TraceId                               |Identifier for a logical transaction | 514b22c0573bf5b992354804a9993cac|
| SpanId                                |Identifier for an individual activity within a trace (see TraceId) | 1b6142c5188baad6|
| ParentId                              |Formatted like span id but the span id of the activity that created the current one (0’s if no parent) | f7ac2e649b1eca3a 0000000000000000|

## Activity Id Format

> W3C 

Industry standard for Trace  Context;
Default for ASP.NET Core in 5+

> Hierarchical

Proprietary to Microsoft; default 
for ASP.NET Core <= 3.1

```c#
    <PackageReference Include="Serilog.Enrichers.Span" Version="2.0.1" />
//ActivityEnricher
builder.Host.UseSerilog((context, loggerConfig) => {
    loggerConfig
    .ReadFrom.Configuration(context.Configuration)
    .WriteTo.Console()
    .Enrich.WithExceptionDetails()
    .Enrich.FromLogContext()
    .Enrich.With<ActivityEnricher>()
    .WriteTo.Seq("http://localhost:5341");
});

builder.Services.AddOpenTelemetryTracing(b => {
        b.SetResourceBuilder(
            ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName)) 
         .AddAspNetCoreInstrumentation()
         .AddHttpClientInstrumentation()         
         .AddOtlpExporter(opts => { opts.Endpoint = new Uri("http://localhost:4317"); });
});
```

## Analyzing Transaction / Application Flow

-    Applications are complex!
-    “The system is slow”
     - “Need more information / detail”
     - But do we, really?
-    Activity tracking can help
-    Application Insights and APM services

```c#
//builder.Logging.AddJsonConsole();
//builder.Services.AddApplicationInsightsTelemetry();
```

## OpenTelemetry

-   Uses the W3C trace context
-   Provides standards to export and collect data from applications
    - Can be “sampled”
-   Covers metrics, logs, and traces
-   Common tools for exporting / viewing:
    - Jaeger
    - Zipkin
    - Prometheus
-   Evolving technology

```c#

builder.Services.AddOpenTelemetryTracing(b => {
        b.SetResourceBuilder(
            ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName)) 
         .AddAspNetCoreInstrumentation()
         .AddHttpClientInstrumentation()         
         .AddOtlpExporter(opts => { opts.Endpoint = new Uri("http://localhost:4317"); });
});

    <PackageReference Include="AspNetCore.HealthChecks.OpenIdConnectServer" Version="6.0.1" />
    <PackageReference Include="OpenTelemetry.Exporter.OpenTelemetryProtocol" Version="1.2.0-rc2" /> 
    <PackageReference Include="OpenTelemetry.Extensions.Hosting" Version="1.0.0-rc9" /> 
    <PackageReference Include="OpenTelemetry.Instrumentation.AspNetCore" Version="1.0.0-rc9" /> 
    <PackageReference Include="OpenTelemetry.Instrumentation.Http" Version="1.0.0-rc9" />

//Api
builder.Services.AddOpenTelemetryTracing(b => {
        b.SetResourceBuilder(
            ResourceBuilder.CreateDefault().AddService(builder.Environment.ApplicationName)) 
         .AddAspNetCoreInstrumentation()
         .AddEntityFrameworkCoreInstrumentation()
         .AddOtlpExporter(opts => { opts.Endpoint = new Uri("http://localhost:4317");  });
});

    public async Task<IEnumerable<Product>> GetProductsForCategoryAsync(string category)
    {               
        _logger.LogInformation("Getting products in logic for {category}", category);

        Activity.Current?.AddEvent(new ActivityEvent("Getting products from repository"));
        var results = await _repo.GetProductsAsync(category);
        Activity.Current?.AddEvent(new ActivityEvent("Retrieved products from repository"));

        return results;
    }
    
```