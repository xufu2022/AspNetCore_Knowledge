# Matching URLs to Razor Pages with routing

routing: which is the process that maps the incoming request to a specific page, or endpoint (incoming URLs), and generates URLs that map to those endpoints (outgoing URLs)

page-based routing : mapping between URLs and file paths as the basis for routing within an application

## Routing basics

The two key components that control routing in a Razor Pages application are the **EndpointRouting** middleware and the **Endpoint** middleware

added to the application pipeline via the UseRouting and MapRazorPages methods

Exception <=> https <=> static <=> routing <=> authorization <=> endpoint

Calling UseRouting before UseEndpoints 

Endpoint middleware is registered at the end of the pipeline and registers endpoints based on Razor Pages conventions.

The role of EndpointRouting middleware is to match incoming URLs to endpoints. In the case of a Razor Pages application, an endpoint is generally a Razor page. . If a match is made, information about the matched endpoint is added to the HttpContext, which is passed along the pipeline.  Once the endpoint has been added to the HttpContext, it can be accessed via the GetEndpoint method of the HttpContext
```c#
app.UseStaticFiles(); ❶
app.UseRouting(); ❷
app.UseAuthorization();
app.MapRazorPages(); 
```

The path of the incoming URL is matched to a collection of routes. The endpoint related to the matched route is added to the HttpContext

The Endpoint middleware is responsible for executing the selected endpoint. If no matching endpoint is found, the last middleware, which is registered in the pipeline by the framework, returns a 404 Not Found HTTP status code

Middleware that does not rely on routing, such as the static file middleware, should be placed prior to the call to UseRouting. Any middleware that needs to know about the selected endpoint is placed between UseRouting and UseEndpoints. Authorization middleware needs to know about the selected endpoint, for example, to determine whether the current user is authorized to access it.

## Route templates

The EndpointRouting middleware attempts to match URLs to endpoints by comparing the path of the URL (the part after the domain) to route templates. A route template is a string representation of a route pattern. The call creates route templates to MapRazorPages in the Endpoint
middleware registration, which instructs the framework to create endpoints based on Razor Pages conventions

It is possible to configure an alternative root for Razor pages through the Razor PagesOptions object during the bootstrapping phase. The following example changes the root directory to Content instead of Pages:

```c#
builder.Services.AddRazorPages().AddRazorPagesOptions(options => {  options.RootDirectory = "/Content";  });
//Or you can use the WithRazorPagesRoot extension method:
builder.Services.AddRazorPages().WithRazorPagesRoot("/Content");
```

so you will need to know how to resolve ambiguous routes

AmbiguousMatchException: \Pages\Privacy\Index.cshtml vs \Pages\Privacy.cshtml

## Customizing route templates

The principal entry point to route template customization is the @page directive in the Razor page itself. The only other thing permitted on the first line of a Razor page file, after the @page directive, is a string that is used to customize the route template for the page

@page "route-template"

As well as literal text, route templates can include two other types of elements: parameters and separators

Parameters are placeholders for dynamic values, like parameters for C# methods, and separators represent the boundary between segments in a URL

The difference between a literal and a parameter is that the latter is enclosed in curly braces. Parameters are an extremely powerful tool in terms of URL-to-endpoint matching

"route-template" / {Parameter}

## Overriding routes

@page "/privacy-policy"

## Route parameters

@page "{cityName}"

Any value passed in to the name parameter is added to a RouteValueDictionary object (literally a dictionary of route values), which is added to a RouteData object
Then, within the page, the value can be retrieved using RouteData.Values

```c#
@page "{cityName}"
<h2>@RouteData.Values["cityName"] Details</h2>
// Default values are assigned to route parameters in the same way they are assigned to normal C# method parameters, albeit without the quotes around string values:
"{cityName=Paris}"
```

## Binding route data to handler parameters

query string values in URLs are automatically bound to parameters in **PageModel** handler methods if their names match. 
The same is true of route data. If a route parameter name matches, the incoming value is automatically assigned to a handler method parameter

assign the parameter value to a PageModel **property**, which is the recommended way to work with route data, rather than accessing the route values dictionary

```c#
@page "{cityName}/{rating?}"
@model CityBreaks.Pages.CityModel
@{
}
<h3>@Model.CityName Details</h3>
<p>Minimum Rating: @Model.Rating.GetValueOrDefault()</p>
```

A catchall parameter is declared by prefixing the name with one or two asterisks
```c#
{*cityName} or {**cityName}

// The difference between using one or two asterisks is apparent when you use the route template to generate URLs
// URL /City/London/2022/4/18 will be rendered as /City%2FLondon%2F2022%2F4%2F18
// two asterisks, the encoding is decoded, or round-tripped, and the generated URL will include literal path separators: /City/London/2022/4/18

```

### Route constraints

Route constraints are applied by separating them from the parameter name with a colon. The following example shows how to constrain each of the date parts to integer types
```c#
"{cityName}/{arrivalYear:int}-{arrivalMonth:int}-{arrivalDay:int}"

"{cityName}/{arrivalYear:int}-{arrivalMonth:range(1-12)}-{arrivalDay:int}"

// multiple constraints
"{cityName:alpha:maxlength(9)}"
```

range constraint accepts minimum and maximum acceptable values

NOTE The Match method in IRouteConstraint is a synchronous method, which makes IRouteConstraint unsuitable for any requirement that should involve asynchronous processing. If you want to constrain incoming routes in a real-world application asynchronously, alternative mechanisms that support this include middleware (see chapter 2) and IEndPointSelectorPolicy (https://learn.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.routing.matching.iendpointselectorpolicy?view=aspnetcore-7.0), which is not covered in this book.


Custom route constraints must be registered
```c#
builder.Services.Configure<RouteOptions>(options =>
{
 options.ConstraintMap.Add("city", typeof(CityRouteConstraint)); ❶
});

// Adding additional route templates via configuration
builder.Services
 .AddRazorPages()
 .AddRazorPagesOptions(options =>
 {
 options.Conventions.AddPageRoute("/Index", "FindMe");
 });
```

There is no limit to the number of additional routes you can define for a particular page using this approach, which takes the name of the page to be mapped and a route template that matches the page. This particular example shows a route template with the literal value "FindMe", but the template applied to the AddPageRoute method can include parameters and constraints just like the ones you have been working with so far

## Working with PageRouteModel conventions directly

PageRouteModelConventions implement the IPageRouteModelConvention interface, which specifies a single method that must be implemented—the Apply method, which takes a PageRouteModel as a parameter

```c#
public interface IPageRouteModelConvention : IPageConvention
{
 void Apply(PageRouteModel model);
}

```

The PageRouteModel parameter provides a gateway for applying new conventions to generate routes for a Razor page. It is through this object that you can apply your custom convention. The PageRouteModel has a **Selectors** property, which represents a collection of SelectorModel objects. Each one of these has an **AttributeRouteModel** property,  which, in turn, has a Template property representing a route template that enables mapping a URL to this particular page

```c#
public class CultureTemplatePageRouteModelConvention : IPageRouteModelConvention ❷
 {
 public void Apply(PageRouteModel model) ❸
 {
 var selectorCount = model.Selectors.Count;
 for (var i = 0; i < selectorCount; i++) ❹
 {
    var selector = model.Selectors[i];
    model.Selectors.Add(new SelectorModel 
    {
        AttributeRouteModel = new AttributeRouteModel
        {
            Order = 100, ❻
            Template = AttributeRouteModel.CombineTemplates("{culture?}",❻selector.AttributeRouteModel.Template) ❻
        }
        });
 }

 builder.Services.AddRazorPages().AddRazorPagesOptions(options => {
 options.Conventions.AddPageRoute("/Index", "FindMe");
 options.Conventions.Add(new CultureTemplatePageRouteModelConvention());
});
```

The custom attributes supported by the anchor tag of most interests to Razor Pages applications are as follows:

- page
- page-handler
- route-*
- all-route-data
- host
- protocol
- area

@page "{handler?}"

The route-* attribute caters to route parameter values, where the * represents the name of the parameter

<a asp-page="/City" asp-route-cityName="Rome">Rome</a>

The **all-route-data** parameter takes a Dictionary<string, string> as a value, which is used to wrap multiple route parameter values. It is provided as a convenience that relieves you from having to add multiple route-* attributes

```c#
var d = new Dictionary<string, string> { { "cityName", "Madrid" },{ "rating", "5" } };
<a asp-page="/City" asp-all-route-data="d">Click</a>
```

### Using the IUrlHelper to generate URLs

The Url property has a number of methods, two of which are of particular interest within a Razor Pages application:
the Page method and the PageLink method

The Page method offers a number of versions or overloads that generate relative URLs as strings from the name of the page
passed in to it, along with the name of a page handler method and route values

The PageLink method generates **absolute** URLs based on the current request

```c#
public void OnGet()
 {
 var target = Url.Page("City", new { cityName = "Berlin", rating = 4 }); //"/City/Berlin/4"

 var target = Url.PageLink("City", values: new { cityName = "Berlin", rating = 4 }); // "https://localhost:5001/City/Berlin/4".

 }

```

### Generating redirect URLs from ActionResults

Two of those helper methods generate URLs that are included in the location header as part of the response: RedirectToPage and RedirectToPagePermanent. Both of these methods are used to instruct the browser to redirect to the generated URL. 

The RedirectToPage method also generates an HTTP 302 status code, which indicates that the location change is temporary. 

In contrast, the RedirectToPagePermanent method generates a 301 HTTP status code, indicating that the redirection should be viewed as representing a permanent change

```c#
public void OnGet()
 {
    return RedirectToPage("City", new { cityName = "Berlin", rating = 4 });
  //It is good practice to set the handler return type as specifically as possible—in this case, RedirectToPageResult
   return RedirectToPage("/Account/Login", new { area = "Identity" });
 }

```

###  Customizing URL generation
```c#
builder.Services.Configure<RouteOptions>(options =>
{
 options.LowercaseUrls = true; 
 options.ConstraintMap.Add("city", typeof(CityConstraint)); 
 options.LowercaseQueryStrings = true;
 options.AppendTrailingSlash = true;
});
```

The RouteOptions object enables you to apply the same case preference for your query strings through its LowercaseQueryStrings property, which is also a Boolean:

The RouteOptions object includes a property named AppendTrailingSlash, which will always result in the slash being appended when set to true:

### Using parameter transformers to customize route and parameter value generation

Parameter transformers are classes that implement the IOutboundParameterTransformer interface that specifies one method: TransformOutbound

The method takes an object as a parameter and returns a string. Despite its name, a parameter transformer can be used to transform the generated page route as well as the parameter values, depending on how it is registered with the application

CityReview becomes City-Review, for example
```c#
public class KebabPageRouteParameterTransformer : ❸ IOutboundParameterTransformer ❸
{ ❸
 public string TransformOutbound(object value) ❸
 {
 if (value == null) ❹
 {
 return null;
 }
 return Regex.Replace(value.ToString(), "([a-z])([A-Z])", "$1-$2"); ❺
 }
}

builder.Services.AddRazorPages().AddRazorPagesOptions(options => {
 options.Conventions.AddPageRoute("/Index", "FindMe");
 options.Conventions.Add(new CultureTemplatePageRouteModelConvention());
 options.Conventions.Add(new PageRouteTransformerConvention(
 new KebabPageRouteParameterTransformer())); 
});


public class SlugParameterTransformer : ❶IOutboundParameterTransformer ❶
{ ❶
 public string TransformOutbound(object value) ❶
 {
 return value?.ToString().Replace(" ", "-"); ❷
 }
}
builder.Services.Configure<RouteOptions>(options =>
{
 options.LowercaseUrls = true;
 options.ConstraintMap.Add("city", typeof(CityConstraint));
 options.ConstraintMap.Add("slug", typeof(SlugParameterTransformer));
});

```

This particular transformer is designed to work on the route of the page, not the parameter values, so it must be registered as a PageRouteTransformerConvention