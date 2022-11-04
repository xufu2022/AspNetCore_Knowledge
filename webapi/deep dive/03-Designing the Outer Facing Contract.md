# Designing the Outer Facing Contract

Resource identifier : http://host/api/authors

HTTP method : https://datatracker.ietf.org/doc/html/rfc9110

Payload : (representation: media types)

## Resource Naming Guidelines

> Nouns: things, not actions
- api/getauthors
- GET api/authors 
- GET api/authors/{authorId} 
> Convey meaning when choosing nouns

> Follow through on this principle for predictability
- api/something/somethingelse/employees :(
- api/employees :)
- api/id/employees :(
- api/employees/{employeeId} :)

> Represent hierarchy when naming resources
- api/authors/{authorId}/courses 
- api/authors/{authorId}/courses/{courseId}
  
> Filters, sorting orders, … aren’t resources
- api/authors/orderby/name :(
- api/authors?orderby=name :)

> Sometimes, RPC-style calls don’t easily map to pluralized resource names
- api/authors/{authorId}/pagetotals :(
- api/authorpagetotals/{id} :(
- api/authors/{authorId}/totalamountofpages :)

## Routing
Routing matches a request URI to an action on a controller

```csharp
    // set up the request pipeline
    ...
    app.MapControllers();
    ...
```

Endpoint Routing

Add endpoints for controller actions to the **IEndpointRouteBuilder** without specifying any routes
Specify routes via route attributes

## Adhering to URI guidelines

> Status codes tell the consumer of the API
- Whether or not the request worked out as expected
- What is responsible for a failed request
> Be as specific as possible
- API consumers are typically non-human
> Be especially specific in regards to reporting who/what is responsible for a mistake

## Learning why Status Codes are Important

- Level 100 Informational
  - mostly unused
- Level 200 Success
  - 200 – Ok
  - 201 – Created
  - 204 – No content
- Level 300 Redirection
  - mostly unused
- Level 400 Client mistakes
  - 400 – Bad request
  - 401 – Unauthorized
  - 403 – Forbidden
  - 404 – Not found
  - 405 – Method not allowed
  - 406 – Not acceptable
  - 409 - Conflict
  - 415 – Unsupported media type
  - 422 – Unprocessable entity
- Level 500 Server mistakes
  - 500 – Internal server error
  
## Errors, Faults and API Availability

> Errors

- Consumer passes invalid data to the API, and the API correctly rejects this
- Level 400 status codes 
- Do not contribute to API availability
  
> Faults
 
- API fails to return a response to a valid request
- Level 500 status codes
- Do contribute to API availability

> Handling faults and avoiding exposing implementation details

## Content negotiation
The process of selecting the best representation for a given response when there are multiple representations available

> Working with Content Negotiation and Formatters

Media type is passed via the Accept header of the request
- application/json
- application/xml
- …

- Returning a representation in a default format when no Accept header is included is acceptable
- Returning a representation in a default format when the requested media type isn’t available isn’t acceptable
  - Return “406 – Not acceptable”

> Output formatter
- Deals with output
- Media type: Accept header

> Input formatter
- Deals with input 
- Media type: Content-type header

> Outer Facing Model vs. Entity Model

- The entity model represents database rows as objects
- The outer facing model represents what’s sent over the wire

## Supporting HEAD

HEAD is identical to GET, with the notable difference that the API shouldn't return a response body 
It can be used to obtain information on the resource
