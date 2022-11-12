## Handling Common Types of Integration

Integrating with an API
- Create (GET)
- Read (POST)
- Update (PUT)
- Delete (DELETE)
- Studying different approaches will lead us to the best practice


## Working with Headers and Content Negotiation

HTTP headers allow passing additional information with each request or response
- name : value
- name : partial value1, partial value2

| **Request headers**    | **Response headers** |
|:-------------------|------------|
|   Contain information on the resource to be fetched, or about the client itself     |   Contain information on the generated response, or about the server   |  
| Are provided by the client   |   Are provided by the server   | 
|   Accept: application/json Accept: application/json, text/html     |   Content-Type: application/json   | 

It’s best practice to be as strict as possible
- For example, setting an Accept header (obligatory in RESTful systems) improves reliability

## Content negotiation
The mechanism used for serving different representations of a resource at the same URI

Content negotiation is driven by
- Accept
- Accept-Encoding
- Accept-Language
- Accept-Charset

### Manipulating request headers

Indicating Preference with the Relative Quality Parameter

- Equal preference
  - Accept: application/json, application/xml
- Indicating preference
  - Accept: application/json, application/xml;q=0.9

## Setting Request Headers

> HttpClient.DefaultRequestHeaders 
- For defaults across requests

> HttpRequestMessage.Headers
- Headers applicable whether or not a request has a body
  
> HttpRequestMessage.Content.Headers
- Headers related to the body of a request

> Inspecting Content Types
- HttpRequestMessage.Content is of type HttpContent
- Use a derived class that matches the content of the message
  - StringContent, ObjectContent, ByteArrayContent, StreamContent, …
  - Optimized for their type of content

# Summary

> Request headers contain more information on the resource to be fetched, or about the client itself
- You are responsible for settings these
> The headers of a response contain information on the generated response or server
- You are responsible for reading these and acting accordingly

> Default values that remain the same across requests
- HttpClient.DefaultRequestHeaders
> Headers that apply to requests regardless of it having a request body
- HttpRequestMessage.Headers
> Headers related to the request body
- HttpRequestMessage.Content.Headers

Shortcuts can come in handy, but if you need full control it’s best to use HttpRequestMessage directly