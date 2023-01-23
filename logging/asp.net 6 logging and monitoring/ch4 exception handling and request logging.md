# Exception Handling and Request Logging

## Exception Handling Principles

- Provide an elegant user experience 
- Shield details from users – don’t help hackers!
- Enable support by providing ID’s and lookup capability
- Rely on your logs during local development
- Use global exception handling and try/catch only when needed

```c#
// Microsoft.AspNetCore.Mvc.ProblemDetails
public class ProblemDetails
{
public string? Type { get; set; } 
public string? Title { get; set; }
public int? Status { get; set; }
public string? Detail { get; set; }
public string? Instance { get; set; }
public IDictionary<string, object?> Extensions { get; }
}

```

 Middleware available in a NuGet package: **Hellang.Middleware.ProblemDetails**

- Update API error handling
- Use Hellang.Middleware.ProblemDetails
- Review handling and logging
- Provide some options to configure
- Middleware for critical error logging
- Update UI to consume ProblemDetails
- Deserialize response 
- Include in log entries

