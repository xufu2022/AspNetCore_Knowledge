# Creating an API and Returning Resources

## Routing

Routing matches a request URI to an action on a controller

> app.UseRouting()

- Marks the position in the middleware pipeline where a routing decision is made
  
> app.UseEndpoints()

- Marks the position in the middleware pipeline where the selected endpoint is executed

```csharp

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints => { 
// map endpoints });

```

### Learning About Routing

Middleware that runs in between selecting the endpoint and executing the selected endpoint can be injected

**Attribute-based Routing**
- No conventions are applied
- This is the preferred approach for APIs

```csharp
app.UseAuthorization();
app.MapControllers();
```

Shortcut: call MapControllers on the WebApplication object directly
- Default in .NET 6
- Mixes request pipeline setup with route management

1. Use attributes at controller and action level: [Route], [HttpGet], …
1. Combined with a URI template, requests are matched to controller actions

For all common HTTP methods, a matching attribute exists
- [HttpGet], [HttpPost], [HttpPatch], …

[Route] doesn’t map to an HTTP method
- Use it at controller level to provide a template that will prefix all templates defined at action level

## The Importance of Status Codes

Common mistakes:
- Don’t send back a 200 Ok when something’s wrong
- Don’t send back a 500 Internal Server Error when the client makes a mistake
- …
