# Improving Performance with Partial Updates

- Http Patch and the JsonPatch standard
- Advanced scenarios
- Switching to Json.NET

## Introducing JsonPatch

PUT is only intended for full updates
Today’s best practice is to use PATCH instead of PUT when updating

The Json Patch standard describes the request body of a PATCH request
- https://tools.ietf.org/html/rfc6902 
- Array of operations (= a change set) 
  
Preferred Content-Type header value: “application/json-patch+json”

```json
[                                       // array of operations
    {
        "op": "replace",                //“replace” operation
        "path": "/title", 
        "value": "new title"            //“title” property gets value “new title”
    }
    , 
    { 
        "op": "remove",                 //“remove” operation
        "path": "/description"          //“description” property is removed (set to its default value)

    }
]
```

**JsonPatch Operations**
> Add
```json
{
    "op": "add",
    "path": "/a/b",
    "value": "foo"
}
```
> Remove
```json
{
    "op": "remove",
    "path": "/a/b"
}
```
> Replace
```json
{
    "op": "replace",
    "path": "/a/b",
    "value": "foo"
}
```
> Copy
```json
{
    "op": "copy",
    "from": "/a/b",
    "path": "/a/c"
}
```
> Move
```json
{
    "op": "move",
    "from": "/a/b",
    "path": "/a/c"
}
```
> Test
```json
{
    "op": "test",
    "path": "/a/b",
    "value": "foo"
}
```

Partially updating resources with PatchAsync
```json
[
    { 
        "op": "add",
        "path": "/actors/3", 
        "value": { 
            "name" : "Jeff Bridges"
            }
    },
    { 
        "op": "replace",
        "path": "/actors/3/nationality", 
        "value": "American"
    }
]
```

**Advanced Patch Scenarios**
Add an actor at position 3 (with the name filled out)
Replace the value of the nationality of the actor at position 3 with a new value

From an API POV, implementing support for this can be complex
Due to this, APIs often stop supporting it after one level deep

** Switching to Json.NET**

System.Text.Json is focused on speed
Json.NET is focused on a set of advanced features
Both are great choices
