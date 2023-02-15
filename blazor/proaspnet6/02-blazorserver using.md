# Using Blazor Server, Part 2

## Combining Components

Blazor components can be combined to create more complex features. In the sections that follow, I show you how multiple components can be used together and how components can communicate.

### Setting and Receiving Bulk Configuration Settings

Defining individual properties to receive values can be error-prone if there are many configuration settings, especially if those values are being received by a component so they can be passed on, either to a child component or to a regular HTML element. In these situations, a single property can be designated to receive any attribute values that have not been matched by other properties, which can then be applied as a set

```html
<select name="select-@Title" class="form-control"
    @bind="SelectedValue" @attributes="Attrs">
        <option disabled selected value="">Select @Title</option>
            @foreach (string val in Values) {
                <option value="@val" selected="@(val == SelectedValue)">
                    @val
        </option>
 }
 </select>
```

```csharp
[Parameter(CaptureUnmatchedValues = true)]
 public Dictionary<string, object>? Attrs { get; set; }
```

## Creating Custom Events and Bindings
```html
<select name="select-@Title" class="form-control"
 @onchange="HandleSelect" value="@SelectedValue">
 ```

 <SelectFilter values="@Cities" title="@SelectTitle" CustomEvent="@HandleCustom" />

```csharp
 [Parameter]
 public EventCallback<string> CustomEvent { get; set; }
 public async Task HandleSelect(ChangeEventArgs e) {
 SelectedValue = e.Value as string;
 await CustomEvent.InvokeAsync(SelectedValue);
 }
```

## Creating a Custom Binding

A parent component can create a binding on a child component if it defines a pair of properties, one of which is assigned a data value and the other of which is a custom event. The names of the property are important: the name of the event property must be the same as the data property plus the word Changed

```csharp
[Parameter]
 public string? SelectedValue { get; set; }

 [Parameter]
 public EventCallback<string> SelectedValueChanged { get; set; }

 public async Task HandleSelect(ChangeEventArgs e) {
    SelectedValue = e.Value as string;
    await SelectedValueChanged.InvokeAsync(SelectedValue);
 }
 ```

Notice that the Parameter attribute must be applied to both the SelectedValue and SelectedValueChanged properties. If either attribute is omitted, the data binding won’t work as expected.

The parent component binds to the child with the @bind-<name> attribute, where <name> corresponds to the property defined by the child component

```html
<SelectFilter values="@Cities" title="@SelectTitle"  @bind-SelectedValue="SelectedCity" />
<button class="btn btn-primary mt-2"  @onclick="@(() => SelectedCity = "Oakland")">  Change </button>
 ```

 ## Displaying Child Content in a Component

 ```csharp
[Parameter]
 public RenderFragment? ChildContent { get; set; }
```

To receive child content, a component defines a property named ChildContent whose type is RenderFragment and that has been decorated with the Parameter attribute. The @ChildContent expression includes the child content in the component’s HTML output. The component in the listing wraps its child content in a div element that is styled using a Bootstrap theme color and that displays a title. The name of the theme color and the text of the title are also received as parameters

### RESTRICTING ELEMENT REUSE

Blazor will reuse an element only if there is a data item that has the same key. For other values, new elements will be created.