# Securing Your API

The authorization code flow with PKCE protection
Getting an access token, passing an access token to an API, validating it
- API scopes vs. API resources

Using access token claims
Including additional identity claims in an access token
Role-based authorization

access_token (as Bearer token in Authorization header)

## (API) scope
The scope of access that's requested by a client

API Scopes vs. API Resources

Typical API scopes:
- read
- write
- ...
  
Use these scopes at level of the API to (dis)allow certain actions

**API Resources**

A physical or logical API

API resource: imagegalleryapi,  employeeapi, 
API scopes: imagegallery.read, customerapi, …imagegallery.write

Web client
API scopes: imagegallery.read, imagegallery.write

Mobile client
API scopes: imagegallery.read

Requesting a scope related to a resource results in: 
- API resource(s) in audience (aud) claim list
- API scope(s) in scopes (scopes) claim list
```json
{
...,
"aud": ["imagegalleryapi"],
"scopes": ["imagegalleryapi.read", "imagegalleryapi.write"]
...
}
```

Sometimes an API needs access to identity claims
- When defining an API resource, include the required claims in the claims list

## Summary


Access tokens are passed to the API as Bearer tokens
- IdentityModel.AspNetCore
  
JwtBearerToken middleware is used to validate an access token at level of the API

Using API resources allows for more complex scenarios than only using API scopes
- Results in audience + scope claim(s)

Configure the API resource to include additional identity claims in the access token

Role-based authorization is achievable through the [Authorize] attribute
