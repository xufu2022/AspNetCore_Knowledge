# Using Model Validation

## Selected ModelStateDictionary Members

 | **Name**    | **Description** |
|:-------------------|------------|
| AddModelError(property, message)    | This method is used to record a model validation error for the specified property. |
| GetValidationState(property)        | This method is used to determine whether there are model validation errors for a specific property, expressed as a value from the ModelValidationState enumeration. |
| IsValid                             | This property returns true if all the model properties are valid and returns false otherwise. |
| Clear()                             | This property clears the validation state. |

## The ValidationSummary Values

 | **Name**    | **Description** |
|:-------------------|------------|
| All         |This value is used to display all the validation errors that have been recorded.|
| ModelOnly   |This value is used to display only the validation errors for the entire model, excluding those that have been recorded for individual properties, as described in the “Displaying Model-Level Messages” section.|
| None        |This value is used to disable the tag helper so that it does not transform the HTML element.|

The **ModelStateDictionary.GetValidationState** method is used to see whether there have been any errors recorded for a model property. The GetValidationState method returns a value from the ModelValidationState enumeration

 | **Name**    | **Description** |
|:-------------------|------------|
| Unvalidated     | This value means that no validation has been performed on the model property, usually because there was no value in the request that corresponded to the property name.|
| Valid           | This value means that the request value associated with the property is valid.|
| Invalid         | This value means that the request value associated with the property is invalid and should not be used.|
| Skipped         | This value means that the model property has not been processed, which usually means that there have been so many validation errors that there is no point continuing to perform validation checks.|

```c#
using Microsoft.AspNetCore.Mvc.ModelBinding;

if (ModelState.GetValidationState(nameof(Product.Price))  == ModelValidationState.Valid && product.Price <= 0) {
    ModelState.AddModelError(nameof(Product.Price), "Enter a positive price");
 }


```

The arguments to the AddModelError method are the name of the property and a string that will be  displayed to the user to describe the validation issue

## Configuring the Default Validation Error Messages

The default messages for some validation errors can be replaced with custom messages using the methods defined by the **DefaultModelBindingMessageProvider** class

 | **Name**    | **Description** |
|:-------------------|------------|
| SetValueMustNotBeNullAccessor        | The function assigned to this property is used to generate a validation error message when a value is null for a model property that is non-nullable.|
| SetMissingBindRequiredValueAccessor  | The function assigned to this property is used to generate a validation error message when the request does not contain a value for a required property.|
| SetMissingKeyOrValueAccessor         | The function assigned to this property is used to generate a validation error message when the data required for dictionary model object contains null keys or values.|
| SetAttemptedValueIsInvalidAccessor   | The function assigned to this property is used to generate a validation error message when the model binding system cannot convert the data value into the required C# type.|
| SetUnknownValueIsInvalidAccessor     | The function assigned to this property is used to generate a validation error message when the model binding system cannot convert the data value into the required C# type.|
| SetValueMustBeANumberAccessor        | The function assigned to this property is used to generate a validation error message when the data value cannot be parsed into a C# numeric type.|
| SetValueIsInvalidAccessor            | The function assigned to this property is used to generate a fallback validation error message that is used as a last resort.|

Each of the methods described in the table accepts a function that is invoked to get the validation message to display to the user.

```c#
builder.Services.Configure<MvcOptions>(opts => opts.ModelBindingMessageProvider
 .SetValueMustNotBeNullAccessor(value => "Please enter a value"));

 <div asp-validation-summary="ModelOnly" class="text-danger"></div>
```

```c#
public IActionResult OnPost() {
if (ModelState.GetValidationState("Product.Price") == ModelValidationState.Valid && Product.Price < 1) {
 ModelState.AddModelError("Product.Price", "Enter a positive price");
 }
 if (ModelState.GetValidationState("Product.Name") == ModelValidationState.Valid  && ModelState.GetValidationState("Product.Price") == ModelValidationState.Valid  && Product.Name.ToLower().StartsWith("small")  && Product.Price > 100) {
 ModelState.AddModelError("",  "Small products cannot cost more than $100");
 }
 if (ModelState.GetValidationState("Product.CategoryId")  == ModelValidationState.Valid &&  !context.Categories.Any(c => c.CategoryId == Product.CategoryId)) {
 ModelState.AddModelError("Product.CategoryId",  "Enter an existing category ID");
 }
 if (ModelState.GetValidationState("Product.SupplierId")  == ModelValidationState.Valid &&  !context.Suppliers.Any(s => s.SupplierId == Product.SupplierId)) {
 ModelState.AddModelError("Product.SupplierId",  "Enter an existing supplier ID");
 }
 if (ModelState.IsValid) {
    TempData["name"] = Product.Name;
    TempData["price"] = Product.Price.ToString();
    TempData["categoryId"] = Product.CategoryId.ToString();
    TempData["supplierId"] = Product.SupplierId.ToString();
    return RedirectToPage("FormResults");
 } else {
    return Page();
 }
}

```

## Specifying Validation Rules Using Metadata

One problem with putting validation logic into an action method is that it ends up being duplicated in every action or handler method that receives data from the user. To help reduce duplication, the validation process supports the use of attributes to express model validation rules directly in the model class, ensuring that the same set of validation rules will be applied regardless of which action method is used to process a request.

- [ ] UNDERSTANDING WEB SERVICE CONTROLLER VALIDATION : Controllers that have been decorated with the ApiController attribute do not need to check the ModelState.IsValid property. Instead, the action method is invoked only if there are no validation errors, which means you can always rely on receiving validated objects through the model binding feature. If any validation errors are detected, then the request is terminated, and an error response is sent to the browser

## Creating a Custom Property Validation Attribute

```c#
public class PrimaryKeyAttribute : ValidationAttribute {
 public Type? ContextType { get; set; }
 public Type? DataType { get; set; }
 protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
    if (ContextType != null && DataType != null) {
        DbContext? context = validationContext.GetRequiredService(ContextType) as DbContext;
    if (context != null && context.Find(DataType, value) == null) {
        return new ValidationResult(ErrorMessage ??  "Enter an existing key value");
        }  
    }
    return ValidationResult.Success;
    }
 }
```

Custom attributes override the IsValid method, which is called with the value to check, and a ValidationContext object that provides context  about the validation process and provides access to the application’s services through its GetService method

## REVALIDATING DATA

- [ ] use the ModelState.Clear method to clear any existing validation errors and call the TryValidateModel method

## Using a Custom Model Validation Attribute in a Razor Page

```c#
public static class ModelStateExtensions {
 public static void PromotePropertyErrors(this ModelStateDictionary modelState,  string propertyName) {
    foreach (var err in modelState) {
        if (err.Key == propertyName &&
            err.Value.ValidationState == ModelValidationState.Invalid) {
            foreach (var e in err.Value.Errors) {
                modelState.AddModelError(string.Empty, e.ErrorMessage);
            }
        }
    }
 }}

 ......

 ModelState.PromotePropertyErrors(nameof(Product));

```

## Performing Remote Validation

```c#
[Remote("CategoryKey", "Validation", ErrorMessage = "Enter an existing key")]
public long CategoryId { get; set; }
 public Category? Category { get; set; }


[HttpGet("categorykey")]
 public bool CategoryKey(string? categoryId, [FromQuery] KeyTarget target) {
 long keyVal;
 return long.TryParse(categoryId ?? target.CategoryId, out keyVal)
 && dataContext.Categories.Find(keyVal) != null;
 }

 [Bind(Prefix = "Product")]
 public class KeyTarget {
 public string? CategoryId { get; set; }
 public string? SupplierId { get; set; }
 }
```
The KeyTarget class is configured to bind to the Product part of the request, with properties that will match the two types of remote validation request. Each action method has been given a KeyTarget parameter, which is used if no value is received for existing parameters. This allows the same action method to accommodate both types of request,
