# Getting Started with REST

## Positioning ASP.NET Core MVC for Building RESTful APIs

- Model-View-Controller is an architectural pattern for implementing user interfaces
- Encourages loose coupling and separation of concerns
- … but it’s not a full application architecture!

REpresentational State Transfer is intended to evoke an image of how a well-designed web application behaves: 
a network of web pages (a virtual state-machine)…
… where the user progresses through an application by selecting links (state transitions)…
… resulting in the next page (representing the next state of the application) being transferred to the user and rendered for their use

## Introducing REST

- REST is an architectural style
- REST is not a standard in its own right
- Standards are used to implement the REST architectural style
- REST is, in principle, protocol agnostic

REST is defined by 6 constraints
- 5 obligatory constraints
- 1 optional constraint

## Constraint
A design decision that can have positive and negative impacts

**Uniform Interface**

API and consumers share one single, technical interface: URI, Method, Media Type (payload)

**Identification of Resources**

A resource is conceptually separate from its representation

Representation media types: 
- application/json
- application/xml
- custom, …

Manipulation of Resources  through Representations

Representation + metadata must be sufficient to modify or delete the resource

Self-descriptive Message : Each message must include enough info to describe how to process the message

**Hypermedia as the Engine of Application State (HATEOAS)**
Hypermedia is a generalization of Hypertext (links)
- Drives how to consume and use the API
- Allows for a self-documenting API

Learning what the REST Constraints Are About

> Uniform Interface
- API and consumers share one single, technical interface: URI, Method, Media Type (payload)

> Client-Server
- client and server are separated(client and server can evolve separately)

> Statelessness
- state is contained within the request
  
> Layered System
- client cannot tell what layer it’s connected to

> Cacheable
- each response message must explicitly state if it can be cached or not

> Code on Demand (optional)
- server can extend client functionality

A system is only considered RESTful when it adheres to all the required constraints…

Most “RESTful” APIs aren’t really RESTful...
… but that doesn’t make them bad APIs, as long as you understand the potential trade-offs

Level 0 (The Swamp of POX)

HTTP protocol is used for remote interaction
… the rest of the protocol isn’t used as it should be
RPC-style implementations (SOAP, often seen when using WCF)

Level 1 (Resources)
POST
http://host/api/authors 
POST
http://host/api/authors/{id} 

Each resource is mapped to a URI
HTTP methods aren’t used as they should be
Results in reduced complexity

Level 2 (Verbs)
GET
http://host/api/authors 
200 Ok (authors)
POST (author representation)
http://host/api/authors 

201 Created (author) 
Correct HTTP verbs are used 
Correct status codes are used
Removes unnecessary variation

Level 3 (Hypermedia) **Level 3 is a precondition for a RESTful API**
GET
http://host/api/authors 
200 Ok (authors + links that drive application state)

The API supports Hypermedia as the Engine of Application State (HATEOAS)
Introduces discoverability

