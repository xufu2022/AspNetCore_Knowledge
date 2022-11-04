# Learning and Implementing HATEOAS

HATEOAS (Hypermedia as the Engine of Application State)

## Hypermedia as the Engine of Application State

Helps with evolvability and self-descriptiveness
- Hypermedia drives how to consume and use the API

```json
{ 
    "id": "5b1c2b4d-48c7-402a-80c3-cc796ad49c6b",
    "title": "Commandeering a ship without getting caught",
    "description": "Commandeering a ship in rough waters ...",
    "authorId": "d28888e9-2ba9-473a-a40f-e38cb54f9b35"
}
```

> Issues Without HATEOAS

- Intrinsic knowledge of the API contract is required 
- An additional rule, or a change of a rule, breaks consumers of the API
- The API cannot evolve separately of consuming applications

```json
{ 
    "id": "5b1c2b4d-48c7-402a-80c3-cc796ad49c6b",
    "title": "Commandeering a ship without getting caught",
    "description": "Commandeering a ship in rough waters ...",
    "authorId": "d28888e9-2ba9-473a-a40f-e38cb54f9b35",
    "numberOfAvailablePlaces": 10
}
```

### Supporting HATEOAS

```json
{ 
    "id": "5b1c2b4d-48c7-402a-80c3-cc796ad49c6b",
    "title": "Commandeering a ship without getting caught",
    "description": "Commandeering a ship in rough waters ...",
    "authorId": "d28888e9-2ba9-473a-a40f-e38cb54f9b35",
    "numberOfAvailablePlaces": 10,
    "links": [
                {
                "href": "http://host/api/authors/{authorId}/courses/{courseId}",
                "rel": "self",
                "method": "GET"
                },
                {
                "href": "http://host/api/authors/{authorId}/courses/{courseId}",
                "rel": "update-course-full",
                "method": "PUT"
                },
                ...,
                {
                "href": "http://host/api/authors/{authorId}/courses/{courseId}",
                "rel": "update-course-partial",
                "method": "PATCH"
                },
            ]
}
```

This is how the HTTP protocol works: leveraging hypermedia
- Links, forms, … drive application state

<a href="uri", rel="type", type="media type">

HTML represents links with the anchor element
- href: contains the uri
- rel: describes how the link relates to the resource
- type: describes the media type

> method 
- defines the method to use
> rel 
- identifies the type of action 
> href 
- contains the URI to be invoked to execute this action

Supporting HATEOAS for Collection Resources
    { 
    "value": [ {author}, { author} ],
    "links": [ … ]
    }

Envelope is required to avoid invalid JSON

This isn’t RESTful when using media type application/json… but we’re fixing that later on

Logic for creating links depends on business rules – requires custom code
- PUT, DELETE, … but also:
- POST to /coursereservations

## Demo Introduction – Supporting HATEOAS

> Statically typed approach

- Base class (with links) and wrapper class
- Inherit base class for single resources
- Use wrapper class for collection resources
  
> Dynamically typed approach

- Anonymous types & ExpandoObject
- Add links to ExpandoObject for single resources
- Use anonymous type for collection resources

**Using HATEOAS for Pagination Links**

```json
{ …
"links": [ …,
    {
    "href": "http://host/api/authors?pageNumber=1&pageSize=10",
    "rel": “previous-page",
    "method": “GET"
    }, 
    {
    "href": "http://host/api/authors?pageNumber=3&pageSize=10",
    "rel": “next-page",
    "method": “GET"
    }
  ]
}

```

## Summary

> HATEOAS
- Hypermedia, like links, drive how to consume and use the API, and the functionality of the consuming application: its state

> HATEOAS diminishes the need for intrinsic API knowledge
- Even if functionality and business rules change, client applications won’t break
