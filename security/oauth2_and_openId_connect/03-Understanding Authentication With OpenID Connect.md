# Understanding Authentication With OpenID Connect

## OpenID Connect for Public Clients 

Angular, React, Vue, Blazor WASM, …
- Consensus towards moving away from handling security at level of the client in favor of the server

Handle flows at level of the hosting application (e.g.: an ASP.NET Core 6 application)
- Potentially use the BFF pattern

Choosing the wrong (variation of a) flow may open you up to security vulnerabilities
- Technically, nothing is stopping us from doing this

## Introducing Duende IdentityServer

OpenID Connect and OAuth 2 framework for ASP.NET Core, developed by Duende Software
- https://duendesoftware.com 
- https://github.com/DuendeSoftware 

## Standardized Scopes and Claims

> Scope: openid (required scope for OIDC)
Claims: sub (user identifier)

> Scope: profile
Claims: name, family_name, given_name, middle_name, nickname, preferred_username, profile, picture, website, gender, birthdate, zoneinfo, locale, updated_at

> Scope: email
Claims: email, email_verified

> Scope: address 
Claims: address

> Scope: phone
Claims: phone_number, phone_number_verified

> Scope: offline_access
Claims: / (used for long-lived access)

A confidential client can safely store secrets, a public client can’t

A flow can be seen as a set of requests and responses via which a client can safely achieve authentication (and authorization)
- For ASP.NET Core web applications, the authorization code flow + PKCE, with client authentication, is advised
