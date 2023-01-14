# install

dotnet new globaljson --sdk-version 7.0.2 --output Platform
dotnet new web --no-https --output Platform --framework net7.0
dotnet new sln -o Platform
dotnet sln Platform add Platform

This method is responsible for setting up the basic features of the ASP.NET Core platform, including creating services responsible for configuration data and logging

This method also sets up the HTTP server, named Kestrel, that is used to **receive HTTP requests**
The result from the CreateBuilder method is a WebApplicationBuilder object, which is used to register additional services
The WebApplicationBuilder class defines a Build method that is used to finalize the initial setup:
```c#
var builder = WebApplication.CreateBuilder(args)
...
var app = builder.Build();
```

**MapGet** is an extension method for the IEndpointRouteBuilder interface, which is implemented by the WebApplication class, and which sets up a function that will handle HTTP requests with a specified URL path

## Creating Custom Middleware
```c#
app.Use(async (context, next) => {
    if (context.Request.Method == HttpMethods.Get
        && context.Request.Query["custom"] == "true") {
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Custom Middleware \n");
        }
    await next();
});
```

The Use method registers a middleware component that is typically expressed as a lambda function that receives each request as it passes through the pipeline

The arguments to the lambda function are an HttpContext object and a function that is invoked to tell ASP.NET Core to pass the request to the next middleware component in the pipeline

 ## Useful HttpContext Members

 | **Name**    | **Description** |
|:-------------------|------------|
| Connection    |This property returns a ConnectionInfo object that provides information about the network connection underlying the HTTP request, including details of local and remote IP addresses and ports. |
| Request    | This property returns an HttpRequest object that describes the HTTP request being processed. |
| RequestServices   | This property provides access to the services available for the request |
| Response    | This property returns an HttpResponse object that is used to create a response to the HTTP request. |
| Session    | This property returns the session data associated with the request |
| User    | This property returns details of the user associated with the request |
| Features    | This property provides access to request features, which allow access to the low-level aspects of request handling|

 ## Useful HttpRequest Members

 | **Name**    | **Description** |
|:-------------------|------------|
| Body          |  This property returns a stream that can be used to read the request body. |
| ContentLength |  This property returns the value of the Content-Length header. |
| ContentType   |  This property returns the value of the Content-Type header. |
| Cookies       |  This property returns the request cookies. |
| Form          |  This property returns a representation of the request body as a form. |
| Headers       |  This property returns the request headers. |
| IsHttps       |  This property returns true if the request was made using HTTPS. |
| Method        |  This property returns the HTTP verb—also known as the HTTP method—used for the request. |
| Path          |  This property returns the path section of the request URL. |
| Query         |  This property returns the query string section of the request URL as key-value pairs. |

## Useful HttpResponse Members

 | **Name**    | **Description** |
|:-------------------|------------|
|ContentLength   |This property sets the value of the Content-Length header.|
|ContentType     |This property sets the value of the Content-Type header.|
|Cookies         |This property allows cookies to be associated with the request.|
|HasStarted      |This property returns true if ASP.NET Core has started to send the response headers to the client, after which it is not possible to make changes to the status code or headers.|
|Headers         | This property allows the response headers to be set.|
|StatusCode      | This property sets the status code for the response.|
|WriteAsync(data)| This asynchronous method writes a data string to the response body.|
|Redirect(url)   | This method sends a redirection response.|

Setting the Content-Type header is important because it prevents the subsequent middleware component from trying to set the response status code and headers. ASP.NET Core will always try to make sure that a valid HTTP response is sent, and this can lead to the response headers or status code being set after an earlier component has already written content to the response body, which produces an exception (because the headers have to be sent to the client before the response body can begin).

- [ ] You may encounter middleware that calls next.Invoke() instead of next(). These are equivalent, and next() is provided as a convenience by the compiler to produce concise code.

## Creating Pipeline Branches

The Map method is used to create a section of pipeline that is used to process requests for specific URLs, creating a separate sequence of middleware components,

```c#
((IApplicationBuilder)app).Map("/branch", branch => {
    branch.UseMiddleware<Platform.QueryStringMiddleWare>();
    branch.Use(async (HttpContext context, Func<Task> next) => {
        await context.Response.WriteAsync($"Branch Middleware");
    });
});

app.UseMiddleware<Platform.QueryStringMiddleWare>();

```

## BRANCHING WITH A PREDICATE
```c#
app.MapWhen(context => context.Request.Query.Keys.Contains("branch"),
 branch => {
 // ...add middleware components here...
});
```

terminal forms
```c#
((IApplicationBuilder)app).Map("/branch", branch => {
    branch.Run(new Platform.QueryStringMiddleWare().Invoke);
});
```

There is no equivalent to the UseMiddleware method for terminal middleware, so the Run method must be used by creating a new instance of the middleware class and selecting its Invoke method