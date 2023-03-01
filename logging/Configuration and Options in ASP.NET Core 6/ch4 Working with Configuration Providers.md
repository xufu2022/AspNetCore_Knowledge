# Working with Configuration Providers

## Overview:

-   Configuration providers
-   How configuration is populated
-   Configuring with environment variables
-   Configuring with command line arguments
-   Securing configuration secrets
    - Development - User secrets
    - Production – Azure Key Vault
-   Configuration from AWS Parameter Store
-   Controlling configuration provider order
-   Defining a custom configuration provider
-   Debugging configuration

## Configuration Providers
- Executed in order to load configuration
- Key/values pairs are added or replaced by each configuration provider
- The order of the configuration providers affects the final configuration values
- A set of default providers are added by the host builder

```csharp
builder.ConfigureAppConfiguration((hostingContext, config) =>
{
    var env = hostingContext.HostingEnvironment;
    config.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true, reloadOnChange: true);
    if (env.IsDevelopment() && !string.IsNullOrEmpty(env.ApplicationName))
    {
        var appAssembly = Assembly.Load(new AssemblyName(env.ApplicationName));
        if (appAssembly != null)
        {
            config.AddUserSecrets(appAssembly, optional: true);
        }
    }
    config.AddEnvironmentVariables();
    if (args != null)
    {
        config.AddCommandLine(args);
    }
})
```

```csharp

//Default Application Configuration Source Order
//Sources can override configuration entries from prior sources
//sequence:

    JSON (appSettings.json)
    JSON (appSettings.Development.json)
    User secrets (in development)
    Environment variables
    Command line arguments

```

## Environment variables

HomePage__ShowGallery = true        HomePage:ShowGallery = true
HideAds = false                     HideAds = true


Define configuration using environment variables
Define environment variables using launchSettings.json

Environment variables are a cross-platform and container-compatible way to provide configuration.

## Command Line Arguments

        dotnet run Features:Greeting:GreetingColour="#000000"
        dotnet run /Features:Greeting:GreetingColour=#000000

## Secure secrets in development

- User secrets CLI
- Managing user secrets

User secrets help protect non-production secrets, during development.

## Debugging Configuration

During development it can be useful to view the final configuration for an application
Debugging allows verification of which configuration values have been applied

###  ConfigurationRootExtensions

Generates a human-readable view of the configuration showing where each value came from.

```csharp
public static string GetDebugView (this IConfigurationRoot root);
```