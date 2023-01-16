# Using Tag Helpers

Tag helpers are C# classes that transform HTML elements in a view or page. Common uses for tag helpers include generating URLs for forms using the application’s routing configuration, ensuring that elements of a specific type are styled consistently, and replacing custom shorthand elements with commonly used fragments of content

## Creating a Tag Helper

```c#
 public class TrTagHelper: TagHelper {
    public string BgColor { get; set; } = "dark";
    public string TextColor { get; set; } = "white";

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        output.Attributes.SetAttribute("class",
        $"bg-{BgColor} text-center text-{TextColor}");
    }
 }
```

Tag helpers are derived from the TagHelper class, which is defined in the Microsoft.AspNetCore 
.Razor.TagHelpers namespace. The TagHelper class defines a Process method, which is overridden by 
subclasses to implement the behavior that transforms elements

- [ ] Asynchronous tag helpers can be created by overriding the ProcessAsync method instead of the Process method, but this isn’t required for most helpers, which tend to make small and focused changes to HTML elements

## Receiving Context Data
Tag helpers receive information about the element they are transforming through an instance of the TagHelperContext class, which is received as an argument to the Process method and which defines the properties


 | **Name**    | **Description** |
|:-------------------|------------|
| AllAttributes   |This property returns a read-only dictionary of the attributes applied to the element being transformed, indexed by name and by index.|
| Items           |This property returns a dictionary that is used to coordinate between tag helpers, as described in the “Coordinating Between Tag Helpers” section.|
| UniqueId        |This property returns a unique identifier for the element being transformed.|

The name of the attribute is automatically converted from the default HTML style, bg-color, to the C# style, BgColor. You can use any attribute prefix except asp- (which Microsoft uses) and data- (which is reserved for custom attributes that are sent to the client)

- [ ] Using the HTML attribute name for tag helper properties doesn’t always lead to readable or understandable classes. You can break the link between the name of the property and the attribute it represents using the HtmlAttributeName attribute, which can be used to specify the HTML attribute that the property represents

## Producing Output

The Process method transforms an element by configuring the TagHelperOutput object that is received as an argument. The TagHelperOuput object starts by describing the HTML element as it appears in the view and is modified through the properties and methods

 | **Name**    | **Description** |
|:-------------------|------------|
| TagName                 | This property is used to get or set the tag name for the output element.|
| Attributes              | This property returns a dictionary containing the attributes for the output element.|
| Content                 | This property returns a TagHelperContent object that is used to set the content of the element.|
| GetChildContentAsync()  | This asynchronous method provides access to the content of the element that will be transformed, as demonstrated in the “Creating Shorthand Elements” section.|
| PreElement              | This property returns a TagHelperContext object that is used to insert content in the view before the output element. See the “Prepending and Appending Content and Elements” section.|
| PostElement             | This property returns a TagHelperContext object that is used to insert content in the view after the output element. See the “Prepending and Appending Content and Elements” section.|
| PreContent              | This property returns a TagHelperContext object that is used to insert content before the output element’s content. See the “Prepending and Appending Content and Elements” section.|
| PostContent             | This property returns a TagHelperContext object that is used to insert content after the output element’s content. See the “Prepending and Appending Content and Elements” section.|
| TagMode                 | This property specifies how the output element will be written, using a value from the TagMode enumeration. See the “Creating Shorthand Elements” section.|
| SupressOuput()          | Calling this method excludes an element from the view. See the “Suppressing the Output Element” section.|

Tag helper classes must be registered with the @addTagHelper directive before they can be used. The set of views or pages to which a tag helper can be applied depends on where the @addTagHelper directive is used

## Narrowing the Scope of a Tag Helper

The range of elements that are transformed by a tag helper can be controlled using the HtmlTargetElement element,

```c#
[HtmlTargetElement("tr", Attributes = "bg-color,text-color", ParentTag = "thead")]
// [HtmlTargetElement("*", Attributes = "bg-color,text-color")]
 public class TrTagHelper : TagHelper {
 }
```
The HtmlTargetElement attribute describes the elements to which the tag helper applies. The first argument specifies the element type and supports the additional named properties

## The HtmlTargetElement Properties

 | **Name**    | **Description** |
|:-------------------|------------|
|Attributes     |This property is used to specify that a tag helper should be applied only to elements that have a given set of attributes, supplied as a comma-separated list. An attribute name that ends with an asterisk will be treated as a prefix so that bg-* will match bg-color, bgsize, and so on.|
|ParentTag      |This property is used to specify that a tag helper should be applied only to elements that are contained within an element of a given type.|
|TagStructure   |This property is used to specify that a tag helper should be applied only to elements whose tag structure corresponds to the given value from the TagStructure enumeration, which defines Unspecified, NormalOrSelfClosing, and WithoutEndTag.|

## Widening the Scope of a Tag Helper

- [ ] ORDERING TAG HELPER EXECUTION: If you need to apply multiple tag helpers to an element, you can control the sequence in which they execute by setting the Order property, which is inherited from the TagHelper base class. Managing the sequence can help minimize the conflicts between tag helpers, although it is still easy to encounter problems

## Advanced Tag Helper Features

Useful TagHelperContent Methods

 | **Name**    | **Description** |
|:-------------------|------------|
| GetContent()        | This method returns the contents of the HTML element as a string.|
| SetContent(text)    | This method sets the content of the output element. The string argument is encoded so that it is safe for inclusion in an HTML element.|
| SetHtmlContent(html)| This method sets the content of the output element. The string argument is assumed to be safely encoded. Use with caution.|
| Append(text)        | This method safely encodes the specified string and adds it to the content of the output element.|
| AppendHtml(html)    | This method adds the specified string to the content of the output element without performing any encoding. Use with caution.|
| Clear()             | This method removes the content of the output element.|

## Creating Elements Programmatically

```c#
using Microsoft.AspNetCore.Mvc.Rendering;
public override async Task ProcessAsync(TagHelperContext context,
 TagHelperOutput output) {
    ......
    TagBuilder header = new TagBuilder("th");
    header.Attributes["colspan"] = "2";
    header.InnerHtml.Append(content);
    TagBuilder row = new TagBuilder("tr");
    row.InnerHtml.AppendHtml(header);
    output.Content.SetHtmlContent(row);
 }
```

## Prepending and Appending Content and Elements

The TagHelperOutput Properties for Appending Context and Elements
 | **Name**    | **Description** |
|:-------------------|------------|
|PreElement  |This property is used to insert elements into the view before the target element.|
|PostElement |This property is used to insert elements into the view after the target element.|
|PreContent  |This property is used to insert content into the target element, before any existing content.|
|PostContent |This property is used to insert content into the target element, after any existing content.|

## Getting View Context Data

A common use for tag helpers is to transform elements so they contain details of the current request or the view model/page model, which requires access to context data
```c#
[HtmlTargetElement("div", Attributes = "[route-data=true]")]
 public class RouteDataTagHelper : TagHelper {
    [ViewContext]
    [HtmlAttributeNotBound]
    public ViewContext Context { get; set; } = new();
    public override void Process(TagHelperContext context, TagHelperOutput output) {
        output.Attributes.SetAttribute("class", "bg-primary m-2 p-2");
        TagBuilder list = new TagBuilder("ul");
        list.Attributes["class"] = "list-group";
        RouteValueDictionary rd = Context.RouteData.Values;
        if (rd.Count > 0) {
            foreach (var kvp in rd) {
                TagBuilder item = new TagBuilder("li");
                item.Attributes["class"] = "list-group-item";
                item.InnerHtml.Append($"{kvp.Key}: {kvp.Value}");
                list.InnerHtml.AppendHtml(item);
            }
            output.Content.AppendHtml(list);
        } else {
            output.Content.Append("No route data");
        }
 }
```

The tag helper transforms div elements that have a route-data attribute whose value is true and populates the output element with a list of the segment variables obtained by the routing system
The **ViewContext** attribute denotes that the value of this property should be assigned a ViewContext object when a new instance of the tag helper class is created, which provides details of the view that is being rendered, including the routing data

The **HtmlAttributeNotBound** attribute prevents a value from being assigned to this property if there is a matching attribute defined on the div element. This is good practice, especially if you are writing tag helpers for other developers to use.

## Working with Model Expressions

```c#
[HtmlTargetElement("tr", Attributes = "for")]
 public class ModelRowTagHelper : TagHelper {
    public string Format { get; set; } = "";
    public ModelExpression? For { get; set; }
 
    public override void Process(TagHelperContext context, TagHelperOutput output) {
        output.TagMode = TagMode.StartTagAndEndTag;
        TagBuilder th = new TagBuilder("th");
        th.InnerHtml.Append(For?.Name ?? String.Empty);
        output.Content.AppendHtml(th);
        TagBuilder td = new TagBuilder("td");
        if (Format != null && For?.Metadata.ModelType == typeof(decimal)) {
            td.InnerHtml.Append(((decimal)For.Model).ToString(Format));
        } else {
            td.InnerHtml.Append(For?.Model.ToString() ?? String.Empty);
        }
        output.Content.AppendHtml(td);
    }
 }
```

This tag helper transforms tr elements that have a for attribute. The important part of this tag helper is the type of the For property, which is used to receive the value of the for attribute

The ModelExpression class is used when you want to operate on part of the view model, which is most easily explained by jumping forward and showing how the tag helper is applied in the view,

```c#
@model Product
 <tr for="Name" />
 <tr for="Price" format="c" />
 <tr for="CategoryId" />
```

The value of the for attribute is the name of a property defined by the view model class. When the tag helper is created, the type of the For property is detected and assigned a ModelExpression object that describes the selected property

## Coordinating Between Tag Helpers

The TagHelperContext.Items property provides a dictionary used by tag helpers that operate on elements and those that operate on their descendants

```c#
[HtmlTargetElement("tr", Attributes = "theme")]
 public class RowTagHelper : TagHelper {

 }

[HtmlTargetElement("th")]
 [HtmlTargetElement("td")]
 public class CellTagHelper : TagHelper {
 }
```
The first tag helper operates on tr elements that have a theme attribute. Coordinating tag helpers can transform their own elements

Tag helpers are not applied to elements generated by other tag helpers and affect only the elements defined in the view

## Suppressing the Output Element

Tag helpers can be used to prevent an element from being included in the HTML response by calling the SuppressOuput method on the TagHelperOutput object that is received as an argument to the Process
method
```c#
[HtmlTargetElement("div", Attributes = "show-when-gt, for")]
 public class SelectiveTagHelper : TagHelper {
    public decimal ShowWhenGt { get; set; }
    public ModelExpression? For { get; set; }
 
    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (For?.Model.GetType() == typeof(decimal) && (decimal)For.Model <= ShowWhenGt) {
            output.SuppressOutput();
        }
    }
 }
```

The tag helper uses the model expression to access the property and calls the SuppressOutput method unless the threshold is exceeded

## Using Tag Helper Components
Tag helper components provide an alternative approach to applying tag helpers as services. This feature can be useful when you need to set up tag helpers to support another service or middleware component, which is typically the case for diagnostic tools or functionality that has both a client-side component and a server-side componenent 

### Creating a Tag Helper Component

```c#
public class TimeTagHelperComponent: TagHelperComponent {

 public override void Process(TagHelperContext context, TagHelperOutput output) {
    string timestamp = DateTime.Now.ToLongTimeString();
    if (output.TagName == "body") {
        TagBuilder elem = new TagBuilder("div");
        elem.Attributes.Add("class", "bg-info text-white m-2 p-2");
        elem.InnerHtml.Append($"Time: {timestamp}");
        output.PreContent.AppendHtml(elem);
    }
}
```

Tag helper components do not specify the elements they transform, and the Process method is invoked for every element for which the tag helper component feature has been configured. By default, tag helper components are applied to transform head and body elements.

This means that tag helper component classes **must check the TagName** property of the output element to ensure they perform only their intended transformations

```c#
using Microsoft.AspNetCore.Razor.TagHelpers;
builder.Services.AddTransient<ITagHelperComponent, TimeTagHelperComponent>();
```