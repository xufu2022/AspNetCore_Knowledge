# Working with Blazor Components

Displaying Data in a Component UI
Technically data binding: one-way data binding from source to target (UI)

Components live in a **namespace**
Root namespace + folder
- Typically project name
May require @using to be added when using the component

## Component Parameters

- Parameters are used to pass data between components
- Use the [Parameter] attribute
- Parameter can be simple or complex type

```razor
<h3>@Name</h3>

@code {
    [Parameter]
    public string Name { get; set; } = string.Empty;
}

// usage
<EmployeeCard Name="Gill Cleeren"></EmployeeCard>
```

## Invoking a Component with a Parameter

- Creating the employee card component
- Passing data using [Parameter]

Working with Events in Components

```razor
@on{Dom Event}="Delegate"

<button @onclick="SaveEmployee">Save</button>
<button @onclick="ShowLocation">Show</button>
@code {
    private void SaveEmployee()
    {
    //save the employee to the backend
    }

    private void ShowLocation(MouseEventArgs e)
    {
    }
```

Using the Default Event Arguments
- @onclick passes MouseEventArgs
- @onkeydown passes KeyboardEventArgs

```razor
    @for (int i = 1; i < 10; i++)
    {
    var buttonNumber = i;
    <p>
        <button @onclick="@(e => ShowLocation(e, buttonNumber))">
            Button @i
        </button>
    </p>
    }
    @code {
        private void ShowLocation(MouseEventArgs e, int buttonNumber)
        {
            //logic
        }
    }

```

## Using EventCallback

Parent Component => Parent event handler
=> Child component => Invoke event callback

```razor
<button @onclick="TriggerCallbackToParent">Show</button>

@code {
    [Parameter]
    public EventCallback<MouseEventArgs> TriggerCallbackToParent { get; set; }
}
```

Using EventCallback in the Nested Child Component

```razor
<ChildComponent TriggerCallbackToParent="ShowPopup"></ChildComponent>
@code {
    private void ShowPopup()
    {
        ... 
    }
}
```

Reacting to an EventCallback in the Parent Component

- Working with the Component Lifecycle
- Events which are triggered automatically at certain points
- Write code in overrides to hook into these

## Important Lifecycle Events

-   OnInitialized() 
-   OnInitializedAsync()
-   OnParametersSet() 
-   OnParametersSetAsync()
-   OnAfterRender() 
-   OnAfterRenderAsync()

```razor
protected override void OnInitialized()
{
    //Initialization code for the component
}
```

## Adding Navigation

Navigating in Blazor Applications

-   Router in App.razor is starting point
-   @page directive enables routing to the component
-   Can accept parameters
-   Use NavigationManager for code-based navigation

Router in App.razor

```razor
<Router AppAssembly="@typeof(App).Assembly">
    <Found Context="routeData">
        <RouteView RouteData="@routeData" DefaultLayout="@typeof(MainLayout)" />
    </Found>
    <NotFound>
        <h1>We can't find your page...</h1>
    </NotFound>
</Router>

```
@page "/employeeoverview"

A Component We Can Navigate to

@page "/employeeoverview“
@page "/employeelist"

### Multiple Page Attributes

```razor
@page "/employeedetail/{EmployeeId}“

[Parameter]
public string EmployeeId { get; set; }
```

Adding Route Parameters
```razor
@page "/employeedetail/{Id:int}“
[Parameter]
public int Id { get; set; }
```

Adding a Constraint
```razor
[Parameter]
[SupplyParameterFromQuery(Name = "id")]
public string EmployeeId { get; set; }
```
> SupplyParameterFromQuery
-   Specify that value can come from query string
-   Name property can be used to define a different query parameter
  
```razor
[Inject]
public NavigationManager NavigationManager { get; set; }
NavigationManager.NavigateTo($"/employeedetail/{selectedEmployee.EmployeeId}");
```

Triggering Navigation from Code
**NavigationManager** is injected here using dependency injection

### Using RenderFragment

Setting the Content

```razor
<ProfilePicture>actual-image-name</ProfilePicture>
```

Using RenderFragment

```razor
<div class="profile-picture">
    @ChildContent
</div>

@code {
    [Parameter]
    public RenderFragment? ChildContent { get; set; }
}
```

[!Note] Naming is everything!
The RenderFragment property must be named **ChildContent**!

## Loading Components Dynamically

Using Dynamic Components

```razor
<DynamicComponent Type="@type" />

@code {
    private Type type = ...;
}
```

- Possible to pass in parameters too
- Can render a dynamic UI if used in a loop

## Handling Errors in Components

The Default Exception Handling in Components
```razor
<ErrorBoundary>
    <EmployeeCard Employee="employee"></EmployeeCard>
</ErrorBoundary>
```

Using Error Boundaries
```razor
<ErrorBoundary>
    <ChildContent>
        <EmployeeCard Employee="employee"></EmployeeCard>
    </ChildContent>
    <ErrorContent>
        <p>Something went wrong!</p>
    </ErrorContent>
</ErrorBoundary>
```

### Using Built-in Components

-   App 
-   Router 
-   DynamicComponent
-   ErrorBoundary 
-   NavMenu 
-   NavLink

> Authentication
> Forms
> Standalone: PageTitle… (Setting the title of the page through Blazor)
