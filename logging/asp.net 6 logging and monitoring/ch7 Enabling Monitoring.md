# Enabling Monitoring

## Monitoring

-   Monitoring an error count
-   Implementing health checks
    - Simple
- AspNetCore.Diagnostics.HealthChecks
    - Custom
-   Monitoring health checks
    - Be careful with logging!
-   Liveness versus readiness
    - Filters

> Something to check

Heartbeat, health check endpoint, transaction times, number of errors

> Something to do the checking

Stethoscope, load balancer, orchestration platform, monitoring service

## Application Performance Monitoring (APM)

- Monitoring – leveled up
- Many logging services provide it
- More layers and areas
    - Infrastructure
    - Methods
    - Database calls
- Some services focus on it
    - AppDynamics
    - Dynatrace
    - NewRelic
    - Stackify

### Demo

- Monitor against log entry queries
- Set up monitors based on number of errors
  - Seq
  - Application Insights

## Health Checks

-   Enable us to define application health
    - Healthy
    - Degraded
    - Unhealthy
-   Endpoint for an HTTP request
-   Can be simple or can include dependencies
-   Don’t forget to monitor them!
-   Demo
    - Simple health check on UI
    - Add health check on API
        - Include DbContext check
    - Add authentication service check in UI
        - AspNetCore.Diagnostics.HealthChecks
        - AspNetCore.HealthChecks.OpenIdConnectServer
        - IHealthCheck interface

```c#
builder.Services.AddHealthChecks()
    .AddDbContextCheck<LocalContext>();
app.MapHealthChecks("health").AllowAnonymous();

  <PackageReference Include="Microsoft.Extensions.Diagnostics.HealthChecks.EntityFrameworkCore" Version="6.0.1" />
```

## Custom Health Checks: IHealthCheck interface
Provide a CheckHealthAsync method that returns a HealthCheckResult and is lightweight

```c#
builder.Services.AddHealthChecks()
    .AddIdentityServer(new Uri("https://demo.duendesoftware.com"), failureStatus: HealthStatus.Degraded);
    <PackageReference Include="AspNetCore.HealthChecks.OpenIdConnectServer" Version="6.0.1" />
```

