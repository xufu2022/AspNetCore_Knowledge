# Supporting Sorting

Sorting resource collections
- One or more fields
- Ascending and descending

## Sorting Collection Resources

> Filtering
- http://host/api/authors?mainCategory=Singing
> Searching
- http://host/api/authors?searchQuery=Kevin
> Paging
- http://host/api/authors?pageNumber=1&PageSize=5


## Sorting Resource Collections

http://host/api/authors?orderBy=age

Resource fields, not entity (or other) fields

http://host/api/authors?orderBy=age desc,name

## Creating a Property Mapping Service

> A resource property might map to multiple properties on underlying objects
- Name maps to FirstName, LastName
> A mapping might require reversing the sort order
- Sorting ascending by Age maps to sorting descending by DateOfBirth

## Property Mapping Service Overview

1. PropertyMappingService : IPropertyMappingService
1. IList<IPropertyMapping> propertyMappings    eg: from AuthorDto to Author
1. PropertyMapping<TSource, TDestination> : IPropertyMapping
1. Dictionary<string, PropertyMappingValue>
1. PropertyMappingValue
1. DestinationProperties   eg: FirstName, LastName
1. Revert  eg: true for Age => DateOfBirth
1. GetPropertyMapping <TSource, TDestination>() eg: from AuthorDto to Author

## Summary

- Collection resources are typically sorted on multiple fields
- Take the sorting direction into account
- Allow sorting on resource fields, not on fields that lie beneath that layer
- Return the OrderBy clause as query string in pagination links
- Errors should be returned as level 400 errors


