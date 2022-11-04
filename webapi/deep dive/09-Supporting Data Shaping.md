# Supporting Data Shaping

Data shaping resources
- Learning about the ExpandoObject
- Shaping collection & single resources
- Handling errors

## Data Shaping Resources

Data shaping allows the consumer of the API to choose the resource fields

Shape on fields of the resource, not on fields of lower-level layer objects

## Creating a reusable extension method to shape data

Data shaping collection resources
Data shaping single resources 
Taking consumer errors into account when shaping data

http://host/api/authors?expand=course
http://host/api/authors?fields=courses.id

### Exploring Additional Options

Including child resources

http://host/api/authors?name.contains(‘Kevin’)

### Advanced filters

## Exploring Additional Options

Don’t implement functionality you don’t reasonably expect consumers of your API to require


# Summary

- Allow data shaping on resource fields, not on fields that lie beneath that layer
- Use ExpandoObject to dynamically create the object to return 
- Use separate methods for shaping single and collection resources for performance reasons
- Return the fields parameter as query string in pagination links
- Errors should be returned as level 400 errors
