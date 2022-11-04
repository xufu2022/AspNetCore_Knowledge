# Manipulating Resources

> Method safety and method idempotency
> Advanced resource creation scenarios
- Parent-child
- Collection in one go
> PATCH vs. PUT
> Upserting
> Supporting OPTIONS
> Inspecting input formatters
> HTTP method overview by use case

## Method Safety and Method Idempotency

A method is considered safe when it doesn't change the resource representation
A method is considered idempotent when it can be called multiple times with the same result


| **HTTP method**    | **Safe?** |  **Idempotent?** | 
|:-------------------|------------|------------|
|   GET     |   Yes   |  Yes   |
| OPTIONS   |   Yes   |  Yes   |
|   HEAD    |   Yes   |  Yes   |
|   POST    |   No    |  No    |
|   DELETE  |   No    |  Yes   |
|   PUT     |   No    |  Yes   |
|   PATCH   |   No    |  No    |

Method safety and idempotency help decide which method to use for which use case

## Advantages of Applying the ApiController Attribute

- Attribute-based routing is obligatory
- A ProblemDetails object is returned in case of mistakes
- Status code 400 (Bad request) is returned on invalid input
- Model binding rules are adjusted to better fit APIs


> [FromBody]
- Request body
- Inferred for complex types
> [FromForm]
- Form data in the request body
- Inferred for action parameters of type IFormFile and IFormFileCollection
> [FromHeader]
- Request header
> [FromQuery]
- Query string parameters
- Inferred for any other action parameters
> [FromRoute]
- Route data from the current request
- Inferred for any action parameter name matching a parameter in the route template
> [FromService]
- The service injected as action parameter

```csharp
[HttpGet("{authorId}", Name = "GetAuthor")]
public async Task<ActionResult<AuthorDto>> GetAuthor(
[FromRoute] Guid authorId){
    ...
}

[HttpPost]
public async Task<ActionResult<AuthorDto>> CreateAuthor(
[FromBody] AuthorForCreationDto author)
```

### Model Binding with Binding Source Attributes

Common for APIs: [FromBody], [FromHeader], [FromQuery] and [FromRoute]

### Full Updates (PUT) vs. Partial Updates (PATCH)

> PUT is for full updates
- All resource fields are either overwritten or set to their default values
> PATCH is for partial updates 
- Allows sending over change sets via JsonPatchDocument
> HTTP PATCH is for partial updates
- The request body of a patch request is described by RFC 6902 (JSON Patch) https://tools.ietf.org/html/rfc6902
> PATCH requests should be sent with media type “application/json-patch+json”

```json
    [
        {
            "op": "replace",
            "path": "/title", 
            "value": “new title" 
        }
        , 
        { 
            "op": “remove",
            "path": "/description"
        }
    ]
```

- array of operations
- “replace” operation
- “title” property gets value “new title”
- “remove” operation
- “description” property is removed (set to its default value

### JSON Patch Operations

```json
 ## Add
    {
        "op": "add",
        "path": "/a/b",
        "value": "foo"
    }
```

```json
## Remove
{
    "op": "remove",
    "path": "/a/b"
}
```

```json
## Replace
{
    "op": "replace",
    "path": "/a/b",
    "value": "foo"
}

```

```json
 ## Copy
{
    "op": "copy",
    "from": "/a/b",
    "path": "/a/c"
}

```

```json
## Move
{
    "op": "move",
    "from": "a/b",
    "path": "/a/c"
}


```

```json

## Test
{
    "op": "test",
    "path": "/a/b",
    "value": "foo"
}


```

### Using PUT or PATCH for Creating Resources: Upserting

**Upserting**

> Server is responsible for URI generation
- PUT/PATCH request must go to an existing URI
- If it doesn’t exist, a 404 is returned
- POST must be used for creation as we cannot know the URI in advance

> Consumer is responsible for URI generation
- PUT/PATCH request can be sent to an unexisting URI, because the consumer is allowed to create it
- If it doesn’t exist, the resource is created
- PUT/PATCH can be used for creation: upserting
