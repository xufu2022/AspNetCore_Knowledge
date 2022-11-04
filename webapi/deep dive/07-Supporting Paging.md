# Supporting Paging

## Paging through Collection Resources

> Collection resources often grow quite large
- Implement paging on all of them 
> Paging helps avoid performance issues

> Parameters are passed through via the query string
- http://host/api/authors?pageNumber=1&pageSize=5

- Limit the maximum page size
- Page by default
- Page all the way through to the underlying data store

## Returning Pagination Metadata

- Must at least include links to the previous and next pages
- Could include additional information: total count, amount of pages, …
  
```json
{ 
    "results": [ 
        {author}, 
        {author}, 
        …],
    "metadata": { 
        "previousPage" : "/api/...", 
        …} 
}
```

## Pagination Metadata

Response body no longer matches the Accept header: this isn’t application/json, it’s a new media type
Breaks the self-descriptive message constraint: the consumer does not know how to interpret the response with content-type application/json

## Returning Pagination Metadata

- When requesting application/json, paging metadata isn’t part of the resource representation
- Use a custom header, like X-Pagination

## Implementing a PagedList<T> Class

> Custom PagedList<T> class
- CurrentPage
- TotalPages
- HasPrevious
- HasNext
> Class can be reused for all collection resources

