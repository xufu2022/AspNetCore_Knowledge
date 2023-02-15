# Using Blazor Server, P1

## Getting Started with Blazor

```csharp
builder.Services.AddServerSideBlazor();
...
app.MapBlazorHub();
//The “hub” in the MapBlazorHub method relates to SignalR, which is the part of ASP.NET Core that 
//handles the persistent HTTP request
```

Adding the Blazor JavaScript File to the Layout
```html
<head>
    ...
    <base href="~/" />
</head>
<script src="_framework/blazor.server.js"></script>
```

## Using a Razor Component
```html
<component type="typeof(Advanced.Blazor.PeopleList)" render-mode="Server" />
```

The render-mode attribute is used to select how content is produced by the component, using a value from the RenderMode enum

 | **Name**    | **Description** | 
|:-------------------|------------|
| Static    | The Razor Component renders its view section as static HTML with no client-side support. | 
| Server    | The HTML document is sent to the browser with a placeholder for the component. The HTML displayed by the component is sent to the browser over the persistent HTTP connection and displayed to the user | 
| ServerPrerendered    | The view section of the component is included in the HTML and displayed to the user immediately. The HTML content is sent again over the persistent HTTP connection. | 


## Understanding the Basic Razor Component Features

```html
<button class="btn btn-primary" @onclick="IncrementCounter">Increment</button>
```
> The EventArgs Classes and the Events They Represent

| **Class**    | **Events** | 
|:-------------------|------------|
| ChangeEventArgs |onchange, oninput|
| ClipboardEventArgs |oncopy, oncut, onpaste|
| DragEventArgs |ondrag, ondragend, ondragenter, ondragleave, ondragover, ondragstart, ondrop|
| ErrorEventArgs |onerror|
| FocusEventArgs |onblur, onfocus, onfocusin, onfocusout|
| KeyboardEventArgs |onkeydown, onkeypress, onkeyup|
| MouseEventArgs |onclick, oncontextmenu, ondblclick, onmousedown, onmousemove, onmouseout, onmouseover, onmouseup, onmousewheel, onwheel|
| PointerEventArgs |ongotpointercapture, onlostpointercapture, onpointercancel, onpointerdown,onpointerenter, onpointerleave, onpointermove, onpointerout, onpointerover, onpointerup |
| ProgressEventArgs |onabort, onload, onloadend, onloadstart, onprogress, ontimeout|
| TouchEventArgs |ontouchcancel, ontouchend, ontouchenter, ontouchleave, ontouchmove, ontouchstart|
| EventArgs |onactivate, onbeforeactivate, onbeforecopy, onbeforecut, onbeforedeactivate, onbeforepaste, oncanplay, oncanplaythrough, oncuechange, ondeactivate, ondurationchange, onemptied, onended, onfullscreenchange, onfullscreenerror, oninvalid, onloadeddata, onloadedmetadata, onpause, onplay, onplaying, onpointerlockchange, onpointerlockerror, onratechange, onreadystatechange, onreset, onscroll, onseeked, onseeking, onselect, onselectionchange, onselectstart, onstalled, onstop, onsubmit, onsuspend, ontimeupdate, onvolumechange, onwaiting |

The Blazor JavaScript code receives the event when it is triggered and forwards it to the server over the persistent HTTP connection. The handler method is invoked, and the state of the component is updated. 
Any changes to the content produced by the component’s view section will be sent back to the JavaScript code, which will update the content displayed by the browser.

```html
<div class="m-2 p-2 border">
 <button class="btn btn-primary" @onclick="@(e => IncrementCounter(e, 1))"> Increment Counter #2
 </button>
 <span class="p-2">Counter Value: @Counter[1]</span>
</div>
```
```csharp
@code {
 public int[] Counter { get; set; } = new int[] { 1, 1 };
 public void IncrementCounter(MouseEventArgs e, int index) {
 Counter[index]++;
 }
}
...

## AVOIDING THE HANDLER METHOD NAME PITFALL

The most common mistake when specifying an event handler method is to include parentheses, 
like this: 
...
<button class="btn btn-primary" @onclick="IncrementCounter()">
...

<button class="btn btn-primary" @onclick="@((e) => HandleEvent(e, local))">

## Processing Events Without a Handler Method

```html
<button class="btn btn-info" @onclick="@(() => Counters.Remove(local))">
 Reset
 </button>
```

Preventing Default Events and Event Propagation
| **Name**    | **Description** | 
|:-------------------|------------|
| @on{event}:preventDefault  | This parameter determines whether the default event for an element is triggered.|
| @on{event}:stopPropagation | This parameter determines whether an event is propagated to its ancestor elements.|

```html
<button class="btn btn-primary"
    @onclick="@(() => IncrementCounter(local))"
    @onclick:preventDefault="EnableEventParams">
 Increment Counter #@(i + 1)
 </button>

 <div class="m-2" @onclick="@(() => IncrementCounter(1))">
    <button class="btn btn-primary" @onclick="@(() => IncrementCounter(0))"  @onclick:stopPropagation="EnableEventParams">Propagation Test</button>
</div>
<div class="form-check m-2">
    <input class="form-check-input" type="checkbox"  @onchange="@(() => EnableEventParams = !EnableEventParams)" />
    <label class="form-check-label">Enable Event Parameters</label>
</div>
```
```csharp
public bool EnableEventParams { get; set; } = false
...

## Working with Data Bindings

Event handlers and Razor expressions can be used to create a two-way relationship between an HTML element and a C# value, which is useful for elements that allow users to make changes, such as input and select elements.

```html
<input class="form-control" @bind="City" />
```
The @bind attribute is used to specify the property that will be updated when the change event is triggered and that will update the value attribute when it changes. The effect in Listing 33-18 is the same as Listing 33-16 but expressed more concisely and without the need for a handler method or a lambda function to update the property

## Changing the Binding Event
By default, the change event is used in bindings, which provides reasonable responsiveness for the user without requiring too many updates from the server. The event used in a binding can be changed by using the attributes

| **Attribute**    | **Description** | 
|:-------------------|------------|
|@bind-value       |  This attribute is used to select the property for the data binding.|
|@bind-value:event |  This attribute is used to select the event for the data binding|

These attributes are used instead of @bind, as shown in Listing 33-19, but can be used only with events that are represented with the ChangeEventArgs class. This means that only the **onchange and oninput** events can be used, at least in the current release

```html
<input class="form-control" @bind-value="City" @bind-value:event="oninput" />
```

## Creating DateTime Bindings

| **Name**    | **Description** | 
|:-------------------|------------|
|@bind:culture |  This attribute is used to select a CultureInfo object that will be used to format the DateTime value.|
|@bind:format  |  This attribute is used to specify a data formatting string that will be used to format the DateTime value.|

```html
<div class="form-group mt-2">
 <label>Time:</label>
 <input class="form-control my-1" @bind="Time" @bind:culture="Culture"  @bind:format="MMM-dd" />
 <input class="form-control my-1" @bind="Time" @bind:culture="Culture" />
 <input class="form-control" type="date" @bind="Time" />
</div>
<div class="p-2 mb-2">Time Value: @Time</div>
<div class="form-group">
 <label>Culture:</label>
 <select class="form-control" @bind="Culture">
    <option value="@CultureInfo.GetCultureInfo("en-us")">en-US</option>
    <option value="@CultureInfo.GetCultureInfo("en-gb")">en-GB</option>
    <option value="@CultureInfo.GetCultureInfo("fr-fr")">fr-FR</option>
 </select>
</div>
```

```csharp
@using System.Globalization
 public DateTime Time { get; set; } = DateTime.Parse("2050/01/20 09:50");
 public CultureInfo Culture { get; set; } = CultureInfo.GetCultureInfo("en-us");
```

LETTING THE BROWSER FORMAT DATES
<input class="form-control" type="date" @bind="Time" />


```csharp
public class CodeOnly : ComponentBase {
 [Inject]
 public DataContext? Context { get; set; }
 protected override void BuildRenderTree(RenderTreeBuilder builder) {
    ...
    builder.OpenElement(1, "button");
    builder.AddAttribute(2, "class", "btn btn-primary mb-2");
    builder.AddAttribute(3, "onclick",
    EventCallback.Factory.Create<MouseEventArgs>(this, () => Ascending = !Ascending));
    builder.AddContent(4, new MarkupString("Toggle"));
    builder.CloseElement();
 ...

 }
```

The base class for components is ComponentBase. The content that would normally be expressed as annotated HTML elements is created by overriding the BuildRenderTree method and using the RenderTreeBuilder parameter