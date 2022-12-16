# Diagnostics, Benchmarking, and Load Testing

## Performance Tools

> Diagnostics

- Measure your app 
- Understand its performance
- Find “hot path”

> Load Testing

See how well your app performs under load conditions

> Benchmarking

Compare two approaches to see which is better

## Key Diagnostics Theme: Know your “Hot Path”

- Start with routes / methods / pages
- Request logging can help
- Chart performance times
  - Alert when slow
- dotnet-trace tool can help
    - Use on running process
    - Captures a lot of data

## Logging and Diagnostics

ILogger interface and Stopwatch class can help

**OpenTelemetry** helps with microservices

Many services provide ready-to-use functionality
- Application Insights (Azure)
- NewRelic
- DataDog
- CloudWatch (AWS)
- Seq
- Elastic Cloud
- … lots more

The dotnet-trace tool can run a trace against a running application

## Key Benchmarking Theme: Compare Approaches

Use a console app
Add BenchmarkDotNet NuGet package - https://benchmarkdotnet.org/
Create methods for different approaches 
Run benchmarking, evaluate results
- Release build
Use what’s faster!

### Benchmarking Caution

Use optimized release builds
Match target processing environment (OS, CPU, language version, etc.)

### Key Load Testing Theme: Iterate!

-   NBomber is simple framework for .NET APIs (and more)
-   JMeter is heavier-weight framework for anything
    - UIs with cookie-based auth
-   Don’t do against production! You’re DoS-ing yourself!
    - Again, use Release builds
-   Thread-count modeling often requires iteration
-   Results need review / interpretation
-   Distributed tests are possible

## Things to Try on your own

> HttpClient

New HttpClient in each request
Call a second API that you have

> Sleep / Delay

Adjust/remove sleeps and evaluate impact

> Database

Remove “NoTracking” behavior
Remove caching

## Summary

- Use Diagnostics to help identify hot path(s)
    - Chart performance over time
    - dotnet-trace can help with running application
- Use Benchmarking to compare performance
    - See which approach is better / more efficient
- Use Load Testing to see how your app behaves under heavier load
    - NBomber is handy .NET framework
    - JMeter is the feature king here