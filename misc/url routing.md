# Using URL Routing

The URL routing middleware components are added to the request pipeline and configured with a set of routes. Each route contains 
a URL path and a delegate that will generate a response when a request with the matching path is received

## Understanding URL Routing

URL routing solves these problems by introducing middleware that takes care of matching request URLs so that components, called endpoints, can focus on responses. The mapping between endpoints and the URLs they require is expressed in a route. The routing middleware processes the URL, inspects the set of routes, and finds the endpoint to handle the request, a process known as routing

## Adding the Routing Middleware and Defining an Endpoint

The routing middleware is added using two separate methods: UseRouting and UseEndpoints. 

The UseRouting method adds the middleware responsible for processing requests to the pipeline. 
The UseEndpoints method is used to define the routes that match URLs to endpoints. 

URLs are matched using patterns that are compared to the path of request URLs, and each route creates a relationship between one URL pattern and one endpoint

```c#
app.UseRouting();
app.UseEndpoints(endpoints => {
    endpoints.MapGet("routing", async context => {
    await context.Response.WriteAsync("Request Was Routed");
 });
});
```

There are no arguments to the UseRouting method. The UseEndpoints method receives a function that accepts an IEndpointRouteBuilder object and uses it to create routes using the extension methods described in Table below

## The IEndpointRouteBuilder Extension Methods

 | **Name**    | **Description** |
|:-------------------|------------|
| MapGet(pattern, endpoint)               |This method routes HTTP GET requests that match the URL pattern to the endpoint.|
| MapPost(pattern, endpoint)              |This method routes HTTP POST requests that match the URL pattern to the endpoint.|
| MapPut(pattern, endpoint)               |This method routes HTTP PUT requests that match the URL pattern to the endpoint.|
| MapDelete(pattern, endpoint)            |This method routes HTTP DELETE requests that match the URL pattern to the endpoint.|
| MapMethods(pattern, methods, endpoint)  |This method routes requests made with one of the specified HTTP methods that match the URL pattern to the endpoint.|
| Map(pattern, endpoint)                  |This method routes all HTTP requests that match the URL pattern to the endpoint.|

Endpoints are defined using RequestDelegate, which is the same delegate used by conventional middleware, so endpoints are asynchronous methods that receive an HttpContext object and use it to generate a response. This means that the features described in Chapter 12 for middleware components can also be used in endpoints

URL patterns are expressed without a leading / character, which isn’t part of the URL path. When the request URL path matches the URL pattern, the request will be forwarded to the endpoint function,

Endpoints generate responses in the same way as the middleware components demonstrated in earlier chapters: they receive an HttpContext object that provides access to the request and response through HttpRequest and HttpResponse objects. This means that any middleware component can also be used as an endpoint

```c#
//app.UseMiddleware<Population>();
//app.UseMiddleware<Capital>();

app.UseEndpoints(endpoints => {
 endpoints.MapGet("routing", async context => {
 await context.Response.WriteAsync("Request Was Routed");
 });
 endpoints.MapGet("capital/uk", new Capital().Invoke);
 endpoints.MapGet("population/paris", new Population().Invoke);
});
```

The **WebApplication** class implements the **IEndpointRouteBuilder** interface, which means that endpoints can be created more concisely. Behind the scenes, the routing middleware is still responsible for matching requests and selecting routes

## Using Segment Variables in URL Patterns

Segment variables, also known as route parameters, expand the range of path segments that a pattern segment will match, allowing more flexible routing. Segment variables are given a name and are denoted by curly braces (the { and } characters)

```c#
app.MapGet("{first}/{second}/{third}", async context => {
 await context.Response.WriteAsync("Request Was Routed\n");
 foreach (var kvp in context.Request.RouteValues) {
 await context.Response
 .WriteAsync($"{kvp.Key}: {kvp.Value}\n");
 }
});
```
When a segment variable is used, the routing middleware provides the endpoint with the contents of the URL path segment they have matched. 
This content is available through the **HttpRequest.RouteValues** property, which returns a **RouteValuesDictionary** object. 

- [ ] There are reserved words that cannot be used as the names for segment variables: **action, area, controller, handler, and page**

## Useful RouteValuesDictionary Members

 | **Name**    | **Description** |
|:-------------------|------------|
| [key]               | The class defines an indexer that allows values to be retrieved by key.|
| Keys                | This property returns the collection of segment variable names.|
| Values              | This property returns the collection of segment variable values.|
| Count               | This property returns the number of segment variables.|
| ContainsKey(key)    | This method returns true if the route data contains a value for the specified key.|

The RouteValuesDictionary class is enumerable, which means that it can be used in a foreach loop to generate a sequence of KeyValuePair<string, object> objects, each of which corresponds to the name of a segment variable and the corresponding value extracted from the request URL

## UNDERSTANDING ROUTE SELECTION

When processing a request, the middleware finds all the routes that can match the request and gives each a score, and the route with the lowest score is selected to handle the route. The scoring process is complex, but the effect is that the most specific route receives the request. This means literal segments 
are given preference over segment variables and that segment variables with constraints are given preference over those without (constraints are described in the “Constraining Segment Matching” section later in this chapter). 

The scoring system can produce surprising results, and you should check to make sure that the URLs supported by your application are matched by the routes you expect. 
If two routes have the same score, meaning they are equally suited to routing the request, then an exception will be thrown, indicating an ambiguous routing selection. See the “Avoiding Ambiguous Route Exceptions” section later in the chapter for details of how to avoid ambiguous routes.

### Generating URLs from Routes

```c#
app.MapGet("capital/{country}", Capital.Endpoint);
app.MapGet("population/{city}", Population.Endpoint)
 .WithMetadata(new RouteNameMetadata("population"));
```

The **WithMetadata** method is used on the result from the MapGet method to assign metadata to the route. The only metadata required for generating URLs is a name, which is assigned by passing a new RouteNameMetadata object, whose constructor argument specifies the name that will be used to refer to the route. The effect of the change in the listing is to assign the route the name population. 

- [ ] Naming routes helps to avoid links being generated that target a route other than the one you expect, but they can be omitted, in which case the routing system will try to find the best matching route.

```c#
case "monaco":
 LinkGenerator? generator = context.RequestServices.GetService<LinkGenerator>();
 string? url = generator?.GetPathByRouteValues(context, "population", new { city = country });
 if (url != null) {
 context.Response.Redirect(url);
 }
 return;

```

The **LinkGenerator** class provides the GetPathByRouteValues method, which is used to generate the URL that will be used in the redirection.

```c#
app.MapGet("capital/{country}", Capital.Endpoint);
app.MapGet("size/{city}", Population.Endpoint)
 .WithMetadata(new RouteNameMetadata("population"));
```

## Managing URL Matching

```c#
app.MapGet("files/{filename}.{ext}", async context => {
 await context.Response.WriteAsync("Request Was Routed\n");
 foreach (var kvp in context.Request.RouteValues) {
 await context.Response
 .WriteAsync($"{kvp.Key}: {kvp.Value}\n");
 }
});
```

## AVOIDING THE COMPLEX PATTERN MISMATCHING PITFALL 
## Using Default Values for Segment Variables
## Using a catchall Segment Variable
```c#
app.MapGet("capital/{country=France}", Capital.Endpoint);
app.MapGet("size/{city?}", Population.Endpoint)

app.MapGet("{first}/{second}/{*catchall}", async context => {
 await context.Response.WriteAsync("Request Was Routed\n");
 foreach (var kvp in context.Request.RouteValues) {
 await context.Response
 .WriteAsync($"{kvp.Key}: {kvp.Value}\n");
 }
});
```

The new pattern contains two-segment variables and a catchall, and the result is that the route will match any URL whose path contains two or more segments

## Constraining Segment Matching

```c#
app.MapGet("{first:int}/{second:bool}", async context => {
        ...
        });
```

The URL Pattern Constraints

 | **Constraint**    | **Description** |
|:-------------------|------------|
|   alpha               |   This constraint matches the letters a to z (and is case-insensitive).   
|   bool                |   This constraint matches true and false (and is case-insensitive).   
|   datetime            |   This constraint matches DateTime values, expressed in the nonlocalized invariant culture format.    
|   decimal             |   This constraint matches decimal values, formatted in the nonlocalized invariant culture.    
|   double              |   This constraint matches double values, formatted in the nonlocalized invariant culture. 
|   file                |   This constraint matches segments whose content represents a file name, in the form name.ext. The existence of the file is not validated.    
|   float               |   This constraint matches float values, formatted in the nonlocalized invariant culture.  
|   guid                |   This constraint matches GUID values.    
|   int                 |   This constraint matches int values. 
|   length(len)         |   This constraint matches path segments that have the specified number of characters. 
|   length(min, max)    |   This constraint matches path segments whose length falls between the lower and upper values specified.  
|   long                |   This constraint matches long values.    
|   max(val)            |   This constraint matches path segments that can be parsed to an int value that is less than or equal to the specified value. 
|   maxlength(len)      |   This constraint matches path segments whose length is equal to or less than the specified value.    
|   min(val)            |   This constraint matches path segments that can be parsed to an int value that is more than or equal to the specified value. 
|   minlength(len)      |   This constraint matches path segments whose length is equal to or more than the specified value.    
|   nonfile             |   This constraint matches segments that do not represent a file name, i.e., values that would not be matched by the file constraint.  
|   range(min, max)     |   This constraint matches path segments that can be parsed to an int value that falls between the inclusive range specified.  
|   regex(expression)   |   This constraint applies a regular expression to match path segments 

Constraining Matching to a Specific Set of Values

```c#
app.MapGet("capital/{country:regex(^uk|france|monaco$)}", Capital.Endpoint);
```

## Defining Fallback Routes

```c#
app.MapFallback(async context => {
 await context.Response.WriteAsync("Routed to fallback endpoint");
});

// MapFallback(endpoint) //This method creates a fallback that routes requests to an endpoint.
// MapFallbackToFile(path) //This method creates a fallback that routes requests to a file.
```
The MapFallback method creates a route that will be used as a last resort and that will match any request

## Advanced Routing Features

### Creating Custom Constraints

```c#
public class CountryRouteConstraint : IRouteConstraint {
 private static string[] countries = { "uk", "france", "monaco" };
 public bool Match(HttpContext? httpContext, IRouter? route, string routeKey, RouteValueDictionary values, RouteDirection routeDirection) {
    string segmentValue = values[routeKey] as string ?? "";
    return Array.IndexOf(countries, segmentValue.ToLower()) > -1;
 }
 }
```

The IRouteConstraint interface defines the Match method, which is called to allow a constraint to decide whether a request should be matched by the route

The parameters for the Match method provide the HttpContext object for the request, the route, the name of the segment, the segment variables extracted from the URL, and whether the request is to check for an incoming or outgoing URL

```c#
builder.Services.Configure<RouteOptions>(opts => {
 opts.ConstraintMap.Add("countryName", typeof(CountryRouteConstraint));
});

app.MapGet("capital/{country:countryName}", Capital.Endpoint);

```

The options pattern is applied to the RouteOptions class, which defines the ConstraintMap property. 

```c#
...
endpoints.MapGet("capital/{country:countryName}", Capital.Endpoint);
...
```

### Avoiding Ambiguous Route Exceptions

```c#
app.Map("{number:int}", async context => {
 await context.Response.WriteAsync("Routed to the int endpoint");
});
app.Map("{number:double}", async context => {
 await context.Response.WriteAsync("Routed to the double endpoint");
});
```

For these situations, preference can be given to a route by defining its order relative to other matching routes,

```c#
app.Map("{number:int}", async context => {
 await context.Response.WriteAsync("Routed to the int endpoint");
}).Add(b => ((RouteEndpointBuilder)b).Order = 1);
app.Map("{number:double}", async context => {
 await context.Response .WriteAsync("Routed to the double endpoint");
}).Add(b => ((RouteEndpointBuilder)b).Order = 2);
```

### Accessing the Endpoint in a Middleware Component

routing is set up by calling the UseRouting and UseEndpoints methods, either explicitly or relying on the ASP.NET Core platform to call them during startup

Although routes are registered in the UseEndpoints method, the selection of a route is done in the UseRouting method, and the endpoint is executed to generate a response in the UseEndpoints method.

Any middleware component that is added to the request pipeline between the UseRouting method and the UseEndpoints method can see which endpoint has been selected before the response is generated and alter its behavior accordingly

```c#
app.Use(async (context, next) => {
 Endpoint? end = context.GetEndpoint();
 if (end != null) {
 await context.Response
 .WriteAsync($"{end.DisplayName} Selected \n");
 } else {
 await context.Response.WriteAsync("No Endpoint Selected \n");
 }
 await next();
});

app.Map("{number:int}", async context => {
 await context.Response.WriteAsync("Routed to the int endpoint");
}).WithDisplayName("Int Endpoint")
 .Add(b => ((RouteEndpointBuilder)b).Order = 1);
app.Map("{number:double}", async context => {
 await context.Response
 .WriteAsync("Routed to the double endpoint");
}).WithDisplayName("Double Endpoint")
 .Add(b => ((RouteEndpointBuilder)b).Order = 2);
```

The GetEndpoint extension method on the HttpContext class returns the endpoint that has been selected to handle the request, described through an Endpoint object.

The Properties Defined by the Endpoint Class

 | **Name**    | **Description** |
|:-------------------|------------|
| DisplayName     | This property returns the display name associated with the endpoint, which can be set using the WithDisplayName method when creating a route.|
| Metadata        | This property returns the collection of metadata associated with the endpoint.|
| RequestDelegate | This property returns the delegate that will be used to generate the response.|

To make it easier to identify the endpoint that the routing middleware has selected, I used the WithDisplayName method to assign names to the routes
The new middleware component adds a message to the response reporting the endpoint that has been selected