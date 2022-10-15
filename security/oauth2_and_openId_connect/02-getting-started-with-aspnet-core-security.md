# Getting Started with ASP.NET Core Security

Application Architectures and Security
- Thick client applications 
    - Windows authentication
- Server-side web applications 
    - Windows or Forms authentication
- Not service-based
- Service-based applications
    - WS-Security (WCF)
    - IP-level configuration (firewall)
- SAML 2.0
    - Standard for exchanging authentication and authorization data between security domains

Client applications (often) require public APIs

Applications that live on the client can’t be (decently) secured with means designed for use at the server

Sending username/password on each request proved to be a bad idea

Token-based security
- Client applications send tokens, representing consent, to API
Home-grown token services emerged…
- The application still has access to username/password

## OAuth2

OAuth2 is an open protocol to allow secure authorization in a simple and standard method from web, mobile and desktop applications

### Introducing OAuth2

A client application can request an access token to gain access to an API
- OAuth2 defines how a client application can securely achieve authorization
- Homegrown endpoints are replaced by endpoints from the OAuth2 standard
- The standard defines how to use these endpoints for different types of client applications
- Identity providers like Duende.IdentityServer, Azure Active Directory, … implement the OAuth2 standard
    - Others include Ping, Okta/Auth0, WSO2 IdentityServer, TrustBuilder, …

### OpenID Connect

OpenID Connect is a simple identity layer on top of the OAuth2 protocol

Introducing OpenID Connect

- A client application can request an identity token (next to an access token)
- That identity token is used to sign in to the client application
- OpenID Connect is the superior protocol: it extends and supersedes OAuth2
- Once you deal with users, use OpenID Connect
- OIDC isn’t just for new or API-based applications