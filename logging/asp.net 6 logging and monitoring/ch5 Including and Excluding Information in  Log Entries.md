# Including and Excluding Information in Log Entries

## Log Method Arguments

_logger.LogLevel(eventId, exception, message, messageArgs);

EventId: Optional numeric id that represents “this type of event”
Exception: The full exception object that should be sent to the log entry – provider will format
Message and Message Args: The text for the message with named, replaceable parameters which are defined by the args

> Event Id
-   Numeric value
-   Not required – use if it helps
-   Define class with events
    - public const int SomeEvent = 1000;
-   Use with “ranges” to isolate feature entries
    - Implies some forethought / organization
    - Example: 
        • 1xxx = browsing products
        • 2xxx = checking out

> Message and Message Args
- string message
    “some text with {paramOne} and maybe {paramTwo}...”
- params object?[] args
    stringVariableOne, complexObjTwo
- Parameters defined by {} in a message are replaced in order by args
    • paramOne = stringVariableOne
    • paramTwo = complexObjTwo.ToString()
- Names of args are not used, only their values
- Names of parameters (e.g. paramOne) are preserved

```c#
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies")
.AddOpenIdConnect("oidc", options =>
{
    options.Authority = "https://demo.duendesoftware.com";
    options.ClientId = "interactive.confidential";
    options.ClientSecret = "secret";
    options.ResponseType = "code";
    options.Scope.Add("openid");
    options.Scope.Add("profile");
    options.Scope.Add("email");
    options.Scope.Add("api");
    options.Scope.Add("offline_access");
    options.GetClaimsFromUserInfoEndpoint = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "email"
    };
    options.SaveTokens = true;
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

var app = builder.Build();

app.UseExceptionHandler("/Error");
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseMiddleware<UserScopeMiddleware>();
app.UseAuthorization();

//note1:
app.MapRazorPages().RequireAuthorization();

app.Run();
```

```c#
if (_httpCtx != null)
            {
                var accessToken = await _httpCtx.GetTokenAsync("access_token");
                _apiClient.DefaultRequestHeaders.Authorization = 
                                new AuthenticationHeaderValue("Bearer", accessToken);
                // for a better way to include and manage access tokens for API calls:
                // https://identitymodel.readthedocs.io/en/latest/aspnetcore/web.html
            }
 var response = await _apiClient.GetAsync($"Product?category={cat}");
            if (!response.IsSuccessStatusCode)
            {      
                 var fullPath = $"{_apiClient.BaseAddress}Product?category={cat}";                
                
                // trace id
                var details = await response.Content.ReadFromJsonAsync<ProblemDetails>() ??
                  new ProblemDetails();
                var traceId = details.Extensions["traceId"]?.ToString();

                LogApiFailure(fullPath, (int) response.StatusCode, traceId ?? "");
               
                //_logger.LogWarning(
                //    "API failure: {fullPath} Response: {apiResponse}, Trace: {trace}, User: {user}",
                //  fullPath, (int) response.StatusCode, traceId, userName);        

                throw new Exception("API call failed!");
            }
```

## Api Applicaion Add EventId and User Information to logEntry

```c#
JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://demo.duendesoftware.com";
        options.Audience = "api";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            NameClaimType = "email"
        };
    });
...
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<LocalContext>();
    context.MigrateAndCreateData();
}

app.UseMiddleware<CriticalExceptionMiddleware>();
app.UseProblemDetails();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.OAuthClientId("interactive.public.short");
        options.OAuthAppName("CarvedRock API");
        options.OAuthUsePkce();
    });
}
app.MapControllers().RequireAuthorization();


[HttpGet]
    public async Task<IEnumerable<Product>> Get(string category = "all")
    {
        using (_logger.BeginScope("ScopeCat: {ScopeCat}", category))
        {     
            LogGettingProducts();       
            //_logger.LogInformation(CarvedRockEvents.GettingProducts, "Getting products in API.");
            return await _productLogic.GetProductsForCategoryAsync(category);
        }
        
        //return _productLogic.GetProductsForCategory(category);
    }
```

## Semantic Logging

- Also called “Structured Logging”
- Strongly typed log entries to create structure
- Enables more precise searching
- Uses parameter names from message templates
- Can destructure objects (vs just ToString())
- JSON formatting is a start

## Scopes

-   Group a set of logical operations
    - Processing a transaction
    - HTTP request
-   Apply via BeginScope(msg, args) method
-   Wrap in a using block
-   Keep your code clean
-   Information available in lower-level entries!

## Demo 

-   Use scopes
    - Category, user in API request
-   Review semantic logging
    - JSON formatting in console
-   Create middleware for user information

```c#
public class UserScopeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserScopeMiddleware> _logger;

    public UserScopeMiddleware(RequestDelegate next, ILogger<UserScopeMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity is { IsAuthenticated: true })
        {
            var user = context.User;
            var pattern = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)";
            var maskedUsername = Regex.Replace(user.Identity.Name??"", pattern, m => new string('*', m.Length));

            var subjectId = user.Claims.First(c=> c.Type == "sub")?.Value;
                        
            using (_logger.BeginScope("User:{user}, SubjectId:{subject}", maskedUsername, subjectId))
            {
                await _next(context);    
            }
        }
        else
        {
            await _next(context);
        }
    }
}
```
```c#

app.UseAuthentication();
app.UseMiddleware<UserScopeMiddleware>();
app.UseAuthorization();

        using (_logger.BeginScope("ScopeCat: {ScopeCat}", category))
        {     
            LogGettingProducts();       
            //_logger.LogInformation(CarvedRockEvents.GettingProducts, "Getting products in API.");
            return await _productLogic.GetProductsForCategoryAsync(category);
        }
        
        //return _productLogic.GetProductsForCategory(category);
```

## Hiding Sensitive Information

- Best policy: don’t log it at all!
    - Redact / mask otherwise
- No silver bullet – it’s mostly up to YOU
- Make sure your team knows what’s sensitive
- Be aware of automatically logged information
    - Cookies, session
    - Request/Response bodies
    - Form content

## LoggerMessage Source Generators

-   Checks if enabled
-   Compiled template rather than parsed / cached
-   Partial void method with params you will log
-   LoggerMessage attribute
    - EventId
    - Log Level
    - Message template

```c#
[LoggerMessage(CarvedRockEvents.GettingProducts, LogLevel.Information, 
        "SourceGenerated - Getting products in API.")]
    partial void LogGettingProducts(); 

[HttpGet]
    public async Task<IEnumerable<Product>> Get(string category = "all")
    {
        using (_logger.BeginScope("ScopeCat: {ScopeCat}", category))
        {     
            LogGettingProducts();       
            //_logger.LogInformation(CarvedRockEvents.GettingProducts, "Getting products in API.");
            return await _productLogic.GetProductsForCategoryAsync(category);
        }
        
        //return _productLogic.GetProductsForCategory(category);
    }

        [LoggerMessage(0, LogLevel.Warning, "API failure: {fullPath} Response: {statusCode}, Trace: {traceId}")]
        partial void LogApiFailure(string fullPath, int statusCode, string traceId);

        public async Task OnGetAsync(){
            ...
            var response = await _apiClient.GetAsync($"Product?category={cat}");
            if (!response.IsSuccessStatusCode)
            {      
                 var fullPath = $"{_apiClient.BaseAddress}Product?category={cat}";                
                
                // trace id
                var details = await response.Content.ReadFromJsonAsync<ProblemDetails>() ??
                  new ProblemDetails();
                var traceId = details.Extensions["traceId"]?.ToString();

                LogApiFailure(fullPath, (int) response.StatusCode, traceId ?? "");
               
                //_logger.LogWarning(
                //    "API failure: {fullPath} Response: {apiResponse}, Trace: {trace}, User: {user}",
                //  fullPath, (int) response.StatusCode, traceId, userName);        

                throw new Exception("API call failed!");
            }
        }
```