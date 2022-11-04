# Supporting Concurrency

- Supporting concurrency in a RESTful world
- Using ETags to support concurrency

## Concurrency Strategies

> Pessimistic concurrency
- Resource is locked
- While it’s locked, it cannot be modified by another client
- This is not possible in REST

> Optimistic concurrency
- Token is returned together with the resource
- The update can happen as long as the token is still valid
- ETags are used as validation tokens

# Summary

Use ETags as tokens/validators for an optimistic concurrency strategy 
- Send as If-Match header value
- On mismatch, 412 Precondition Failed will be returned
