# Adding Background Tasks to ASP.NET Core Applications

## Creating a Hosted Service

- Periodically perform background work

### Hosted Services

Provide a clean pattern for executing code, within an ASP.NET Core application, outside of the request pipeline. Each service is started as an asynchronous background Task. 

> Use Cases

- Polling for data from an external service
- Responding to external messages or events
- Performing data-intensive work, outside of the request lifecycle

## Background Service

Abstract base class for creation of background services

```csharp
public abstract class BackgroundService : IHostedService, IDisposable
{
    protected BackgroundService();
    public virtual void Dispose();
    public virtual Task StartAsync(CancellationToken cancellationToken);
    public virtual Task StopAsync(CancellationToken cancellationToken);
    protected abstract Task ExecuteAsync(CancellationToken stoppingToken);
}
```

Coordinate between requests and hosted services
- Use ‘Channels’ to transfer data
- Improve response times

## Summary

- IHostedService interface
- BackgroundService abstract class
- Implemented a hosted service for cache population
- Passed data to hosted services from web requests
    - Used a channel to transfer request data to a hosted service

