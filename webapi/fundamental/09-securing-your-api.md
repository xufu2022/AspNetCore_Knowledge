# securing api

## Token-based security

- Send a token on each request
- A token represents consent
- Validate the token at level of the API

Approach works for almost all modern application types

## Implementing Token-based Security

API “login” endpoint accepting a username/password


eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c

{
"sub": "1234567890",
"name": "John Doe",
"iat": 1516239022
}

> Payload
E.g.: some JSON that contains generic token info, like when the token was created, and some 
info about the user

> Signature
A hash of the payload, used to ensure the data wasn’t tampered with

> Header
Essential token information like the key algorithm used for signing

- API “login” endpoint accepting a username/password
    - POST api/login
- Ensure the API can only be accessed with a valid token
- Pass the token from the client to the API as a Bearer token on each request
    - Authorization: Bearer mytoken123

> Working with Authorization Policies

Authorization policies help with building a full-fledged authorization layer
- Avoids having to enter the actual controller action

> ABAC/CBAC/PBAC

- Access rights granted through policies
- A policy combines a set of attributes (claims) together
- Allows much more complex rules than RBAC (Role-based Access Control)

**Using information from the token in an authorization policy**

**OAuth2**
OAuth2 is an open protocol to allow secure authorization in a simple and standard method from web, mobile and desktop applications

**OpenID Connect**
OpenID Connect is a simple identity layer on top of the OAuth2 protocol

