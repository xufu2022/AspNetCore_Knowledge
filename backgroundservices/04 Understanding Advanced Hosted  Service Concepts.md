# Understanding Advanced Hosted Service Concepts

## BackgroundService Implementation Details

- Add exception handling
- Why exception handling is important

## Hosted Service Unhandled Exception Behavior

- Since .NET 6, by default, unhandled exception cause the application to shutdown
- In prior .NET versions, the application would continue to run
- Unhandled exceptions represent a failure that cannot be automatically recovered
- Exceptions are logged before shutdown
- Unhandled exceptions should not be ignored
- The shutdown behavior can be configured

> Hosted Service

- How lifetime is managed by the host
- How the shutdown process works
- Trigger shutdown
- Configure shutdown options

## IHostApplicationLifetime

Allows consumers to be notified of application lifetime events.

```csharp
public interface IHostApplicationLifetime
{
    CancellationToken ApplicationStarted { get; }
    CancellationToken ApplicationStopping { get; }
    CancellationToken ApplicationStopped { get; }
    void StopApplication();
}
```

It’s safe to call the StopApplication method multiple times.

## Registration Order of Background Services

The registration order of background services can be important.

Starting and Stopping Background Services

StartAsync() => ServiceA.StartAsync() => ServiceB.StartAsync() => ServiceC.StartAsync()
=> ServiceC.StopAsync() => ServiceB.StopAsync() => ServiceA.StopAsync() => StopAsync()
 
## Configuring the Host

There are other methods we can call on the HostBuilder to further configure the host.

    Host.CreateDefaultBuilder()

> HostBuilder Defaults

 | **Name**    | **Description** |
|:-------------------|------------|
| AddModelError(property, message)    | This method is used to record a model validation error for the specified property. |
| GetValidationState(property)        | This method is used to determine whether there are model validation errors for a specific property, expressed as a value from the ModelValidationState enumeration. |
| IsValid                             | This property returns true if all the model properties are valid and returns false otherwise. |
| Clear()                             | This property clears the validation state. |

 | **Name**    | **Description** |
|:-------------------|------------|
|Loads application configuration    |  Adds logging providers     |
|appSettings.json                   |  Console     |
|appsettings.{Environment}.json     |  Debug     |
|Secret Manager (when the app runs  in the development environment) |   EventSource  EventLog (Windows only)    |
|Environment variables              |       |
|Command-line arguments             |       |

```csharp
IHost host = Host.CreateDefaultBuilder()
    .ConfigureServices(services =>
    {
        services.AddSingleton<ISomething, Something>();
    })
    .ConfigureAppConfiguration(config =>
    {
        config.AddInMemoryCollection(new[]
        {
        new KeyValuePair<string, string>("Key", "Value")
        });
    })
    .ConfigureLogging(logging =>
    {
        logging.AddSimpleConsole(opt => opt.IncludeScopes = true);
    })
.Build();
```

Override StartAsync and StopAsync
- Add timer logging to StopAsync


When inheriting from BackgroundService, you won’t often need to override StartAsync or StopAsync.

## Unit testing background services

Extract most functionality into targeted and more easily testable classes with a single responsibility.

### StartAsync

Avoiding Blocking Code in StartAsync

Each hosted service is started sequentially by awaiting its StartAsync method.

- StartAsync blocks application startup
- StartAsync should complete quickly
- Avoid long-running work in StartAsync
- ExecuteAsync also blocks startup until the first await
- Awaiting an immediately completed Task is effectively synchronous

## Summary

- Handling exceptions
- Triggering and handling shutdown
- Registration order of hosted services
- Overriding StartAsync and StopAsync
- Unit testing hosted services
- Avoiding startup delays in hosted services