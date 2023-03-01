# Applying the Options Pattern

## Overview

-   Introduce the options pattern
    - IOptions<T>
    - IOptionsSnapshot<T>
    - IOptionsMonitor<T>
-   Use named options
-   Apply options validation
-   Unit testing dependent types
-   Apply the options pattern
-   Bind configuration to strongly typed options classes
-   Inject options with IOptions<T>

## IOptions<T>

- Does not support options reloading 
- Registered as a singleton in D.I. container
- Values bound when first used
- Can be injected into all service lifetimes
- Does not support named options


## IOptionsSnapshot <T>

- Supports reloading of configuration
- Registered as scoped in D.I. container
- Values may reload per request
- Can not be injected into singleton services
- Supports named options

## IOptionsMonitor<T>

- Supports reloading of configuration
- Registered as a singleton in D.I. container
- Values are reloaded immediately
- Can be injected into all service lifetimes
- Supports named options

## Choosing an Options Interface

-   Options validation
    - Data annotation attributes
-   When is validation applied?
-   Ensuring options are valid at start up
-   Advanced validation techniques
    - Define conditional validation logic
    - Implement IValidateOptions

## Validating named options

Forwarding to options via an interface

Unit testing types dependent on options classes
- Using Options.Create
- Mocking with Moq
- Using IServiceProvider

## Faq:  Used named options