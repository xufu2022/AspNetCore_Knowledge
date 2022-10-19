# Understanding Blazor

Blazor is a framework to build interactive web UIs using C# and HTML. It’s part of ASP.NET Core.

- Based on WebAssembly (WASM) or run on server
- No plugin, based on web standards
- Integrate with JavaScript
- Benefits of Visual Studio and .NET including performance and libraries

## Components in Blazor

- “Everything” is a component in Blazor
- UI and logic
- Created using HTML and C#

## The Blazor Hosting Models

> WebAssembly
-   Client-side, in-browser
-   Compiled application is downloaded, along with .NET runtime
-   Deployed as static files

-   Running a Blazor 
-   App as a PWA
-   Take advantage of specific APIs of the browser
-   Occasionally-connected apps
> Server
-   Executes on server in ASP.NET Core app 
-   Backed by SignalR connection
-   Application instance is created per “circuit”
-   
-   Small download
-   Works with all server-side APIs
-   Blazor apps even in non-supported browsers
  
> Disadvantages of Blazor Server
-   No offline support
-   Network delay
-   Scalability

> Hybrid
-   Native desktop app
-   Native mobile app with Maui
-   
-   Access to native device features
-   Share code between web, mobile and even desktop
-   Different apps for different platforms though

General:
-   Runs on all modern browsers
-   No .NET required on server
-   SPA user experience
-   But…
-   - Not supported on “really old browsers”
-   - Initial download may take longer


