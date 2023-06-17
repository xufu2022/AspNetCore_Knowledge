# Building .NET Worker Services

-   Worker services
    - Microservice architecture
-   Create a new worker service project
-   Migrate processing to a worker service
    - Poll a message queue
    - Load and process the results file
    - Refactor the web application

## What Are Worker Services?

Worker Services

- Console application 
- Hosting supports long-running workloads
- Scheduled workloads

### .NET Hosting

- Logging 
- Dependency Injection 
- Configuration

Worker services can be applied as a core component to build cloud-native, microservice architectures.

## Common Workloads

- Processing messages/events from a queue, service bus or event stream
- Reacting to file changes in a object/file store
- Aggregating data from a data store
- Enriching data in data ingestion pipelines
- Formatting and cleansing of AI/ML datasets

dotnet new worker -n “ExampleWorkerService"

### .NET CLI Command
Creates a new .NET project using the worker service template

## Hosting in .NET

- Manages application lifetime
- Provides components such as dependency injection, logging and configuration
- Turns a console application into a long-running service
- Starts and stops hosted services

The Kestrel web server is started as a hosted service.

## Creating a Host

```csharp
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
    services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
```

## Worker Service Architecture

Objectives

- Remove data processing from web application
- Break off responsibilities to microservices
- Design for cloud hosting in AWS

## Amazon Web Services (AWS)

- Simple Storage Service (S3)
- Simple Notification Services (SNS)
- Simple Queue Service (SQS)

## Decoupling Services with Queues

# Summar

-   Worker service template
-   Built a worker service
    - Replaced web application processing
    - Created a .NET microservice
-   Host provides features such as 
    - Dependency injection
    - Logging 
    - Configuration