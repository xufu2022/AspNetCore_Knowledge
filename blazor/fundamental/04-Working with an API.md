# Working with an API

## Interacting with the REST API

- HttpClient 
- IHttpClientFactory

## Sidestep: Dependency Injection (DI)

    DI Container => LoggerService =>ILoggerService => MyClass =< DI Container

```csharp
builder.Services.AddScoped(sp =>
    new HttpClient
    {
        BaseAddress = new Uri("http://<your-api-endpoint>")
    }
);
```

## Registering the HttpClient

```csharp
[Inject] 
public HttpClient HttpClient { get; set; }
```

Accessing the HttpClient in a Component

```csharp
protected override async Task OnInitializedAsync()
{
    Employees = await HttpClient.GetFromJsonAsync<Employee[]>("api/employee");
}

```

Available Methods

- GetFromJsonAsync() 
- ostAsJsonAsync()
- PutAsJsonAsync() 
- DeleteAsync()

## IHttpClientFactory

- Used to configure and create HttpClient instances in a central location
- Support for named and typed HttpClient
- Requires Microsoft.Extensions.Http package

```csharp

builder.Services.AddHttpClient<IEmployeeDataService, EmployeeDataService>
(client => client.BaseAddress = new Uri("https://localhost:44340/"));

```

# Registering in the Program
**Typed client is used here**

Creating a Service Class

```csharp

public class EmployeeDataService : IEmployeeDataService
{
    private readonly HttpClient _httpClient;

    public EmployeeDataService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}

```

## Managing the Application State

> Application state
- Blazor WASM is an application that’s running in the memory of the browser
- By default, each component is an island that’s recreated every time

Blazor app (in memory of browser)

Component 1 => “State” => Component 2

```csharp
public class ApplicationState
{
    public int NumberOfMessages { get; set; } = 0;
}
```

Creating an Application State Class

```csharp

builder.Services.AddScoped<ApplicationState>();

```

## Adding an Instance to the DI Container

```csharp

[Inject]
public ApplicationState? ApplicationState { get; set; }

int a = ApplicationState.NumberOfMessages;

```

## Accessing the Application State from Components

Application State

This type of state is in-memory and will be removed when the application restarts!

### Adding application state

## Storing Data Locally

- Made possible through the browser, accessible using JavaScript
- SessionStorage or LocalStorage
- Possible to use from Blazor WASM too

Using Blazored LocalStorage

```csharp
    @inject Blazored.LocalStorage.ILocalStorageService localStorage
    var firstName = await localStorage.GetItemAsync<string>("EmployeeFirstName");
```

### Using ILocalStorageService

Available APIs

- SetItem()
- SetItemAsync()
- GetItem()
- GetItemAsync()
- ContainKey()
- ContainKeyAsync()
- RemoveItem()
- RemoveItemAsync()

Adding the Blazored.LocalStorage package
Extending the service with local storage support
