# Improving Reliability with Advanced Content Negotiation

> Revisiting the contract between client and server
> Advanced content negotiation
- Vendor-specific media types (input and output)
> Versioning in a RESTful world
- Should RESTful APIs be versioned?

## Revisiting the Contract Between Client and Server

application/json tells us something about the format of the data, but not about the type

> HATEOAS and Content Negotiation

- Self-descriptive message sub constraint
  - Each message must include enough info to process it
- We’re returning the wrong representation
  - We’re not as strict as we could be

## Vendor-specific Media Types

application/vnd.marvin.hateoas+json

- Top-level type : application
- Vendor prefix: vnd
- Vendor identifier: marvin
- Media type name: hateoas
- Suffix: json

## Semantic media types

Media types that tell something about the semantics of the data – e.g.: what the data means
- Vendor-specific media types

## Combining Semantic Media Types with HATEOAS

> application/vnd.marvin.author.friendly+json
- Friendly representation without links
> application/vnd.marvin.author.friendly+hateoas+json
- Friendly representation with links
> application/vnd.marvin.author.full+json
- Full representation without links
> application/vnd.marvin.author.full+hateoas+json
- Full representation with links

There should be only one suffix per media type, and only officially registered suffixes should be used

Always provide a default representation that will be returned when no semantic information is passed through 
- e.g.: application/json

## Working with Vendor-specific Media Types on Input

> Working with vendor-specific media types on output
> When inputting data we can use vendor specific media types as well through the Content-Type header
> appliation/json & application/vnd.marvin.authorforcreation+json. 
- Representation without date of death
> application/vnd.marvin.authorforcreationwithdateofdeath+json
- Representation with date of death


## Improving resource representation selection with an ActionConstraint

> Versioning in a RESTful World
- Functionality
- Business rules
- Resource representations

## Versioning in a RESTful World

> Through the URI
- api/v1/authors
> Through query string parameters
- api/authors?api-version=v1
> Through a custom header
- “api-version”=v1

> Evolvability
- Use HATEOAS to adapt to changes in functionality & business rules
- Use CoD (Code on Demand) to adapt to changes in media types/resource representations

Version media types to handle change in representations
- application/vnd.marvin.author.friendly.v1+json
- application/vnd.marvin.author.friendly.v2+json

… or use friendly names

## Summary

Use vendor-specific media types to differentiate between resources with and without HATEOAS links

Use semantic media types (implemented with vendor-specific media types) to attach meaning to representation requests
- Improves evolvability and reliability

Adapting to change
- HATEOAS for changes to functionality and business rules
- Versioned media types (until code on demand is feasible)

The REST architectural style was created with systems in mind that should live for years or decades, not months
