# Understanding Authorization with OpenID Connect

## Learning How OAuth2 Works

OpenID Connect standardized claims & verification methods
- You’re often using OpenID Connect, even when you only require an access token

> Using OpenID Connect for Authentication and Authorization
1. Client application (relying party) 
1. IDP
1. Client application
1. API
   
> OAuth2 and OpenID Connect Flows
1. Authorization code 
1. Implicit 
1. Hybrid (OIDC only)

> Resource Owner Password Credentials (OAuth2 only)
- In-app login screen
- Only for trusted applications
- Should be avoided

> Client Credentials (OAuth2 only)
- No user involvement
- Confidential clients
- For machine to machine communication

```json
{ 
"sub": "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
"iss": "https://localhost:5001",
"aud": [
"imagegalleryapi“,
"https://localhost:5001/resources"],
…
}
```

Access tokens are often JWTs, but don’t have to be (e.g.: reference tokens)
The intended audience 
- Our image gallery API 
- Resources at level of the IDP (e.g. when calling the UserInfo endpoint)
- The client identifier signifies the client application that requested the access token
- The scopes in this token give access to API resources and Identity resources

Use OIDC for authentication and authorization (it extends & improves on OAuth2)
The advised flow is the authorization code flow with PKCE protection

OAuth2-only flows
- ROPC must be avoided
- Client credentials is for machine to machine communication


