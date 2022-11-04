# Validating Data and Reporting Validation Errors

- Working with validation in a RESTful world
- Reporting validation problems
  - Problem details RFC

## Working with Validation in a RESTful **World**

- Defining validation rules
- Reporting validation errors
- Checking validation rules

> Defining validation rules

- In ASP.NET Core rules are defined through
  - Data annotations (built-in or custom)
  - IValidatableObject
- Focus on input validation

> Checking validation rules

- ModelState
  - A dictionary containing both the state of the model, and model binding validation 
  - Contains a collection of error messages for each property value submitted
- ModelState.IsValid()
  
> Reporting Validation Errors

- Response status should be 422 
  - Unprocessable entity
- Response body should contain validation errors
  - Problem details RFC

> Validating input with data annotations

When a controller is annotated with the [ApiController] attribute it will automatically return a 400 Bad Request on validation errors

Problem details for HTTP APIs RFC (https://tools.ietf.org/html/rfc7807)
- Defines common error formats for those applications that need one 
- Allows identifying distinct problem types specific to API needs
  
**Reporting Validation Errors**

Content-Type: application/problem+json

```json
{
"errors": {
    "title": [
        "The title shouldn't have more than 100 characters."
            ] 
        },
"type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
"title": "One or more validation errors occurred.",
"status": 400,
"traceId": "00-f303f218c960a22c1d820b6478b4af5f-fd233fa633c4709e-00"
}


{
"errors": {
    "title": [
    "The title shouldn't have more than 100 characters."
    ]},
"type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
"title": "One or more validation errors occurred.",
"status": 422,
"detail": "See the errors property for details.",
"instance": "/api/authors/2902b665-1190-4c70-9915-b9c2d7680450/courses",
"extensions": {
"traceId": "0HLO3MNBSPFI2:00000001"
}}


```

Class-level input validation with IValidatableObject
Class-level input validation with a custom attribute
Validating input when updating a resource with PATCH


# Summary

> Define the rules
- Data annotations
- IValidatableObject
> Check the rules
- ModelState.IsValid()
- Automated thanks to [ApiController]
> Report errors
- 422 Unprocessable entity
- Problem details RFC
