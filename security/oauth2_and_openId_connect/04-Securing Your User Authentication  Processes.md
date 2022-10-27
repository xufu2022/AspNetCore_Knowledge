# Securing Your User Authentication Processes

The authorization code flow with PKCE protection
- Logging in and logging out
Best practice for returning identity claims

## The Authorization Code Flow

https://idphostaddress/connect/authorize?client_id=imagegalleryclient&redirect_uri=https://clientapphostaddress/signin-oidc&scope=openid profile&response_type=code&response_mode=form_post&nonce=63626...n2eNMxA0

Authentication request to the authorization endpoint 
- Authorization endpoint at IDP level - https://idphostaddress/connect/authorize?
- Identifier of the client - client_id=imagegalleryclient
- Redirection endpoint at client level - https://clientapphostaddress/signin-oidc
- Requested scopes by the client application - openid profile 
- The requested response_type determines the flow -code


**Response Type Values**
> code
- Authorization Code : A very short-lived token that provides proof of authentication, linked to the user that just signed in to the IDP

Communication Types

>Front channel communication
- Information delivered to the browser via URI or Form POST (response_mode)
- In our current flow: authorization endpoint

> Back channel communication
- Server to server communication
- In our current flow: token endpoint

### Proof Key for Code Exchange (PKCE)

Mitigate with the PKCE (Proof Key for Code Exchange) approach
- https://tools.ietf.org/html/rfc7636
- For each request to the auth endpoint, a secret is created
- When calling the token endpoint, it’s verified

Code injection is mitigated because the attacker doesn’t have access to the perrequest secret

### The UserInfo Endpoint

Not including the claims in the id_token
- Keeps the token smaller, avoiding URI length restrictions
- Decreases the potential gains of an attack in case of token interception
- Used by the client application to request additional user claims
- Requires an access token with scopes related to the claims that have to be returned

The Authorization Code Flow + PKCE + UserInfo

1. Client application (relying party) => token request (code, clientid, clientsecret, code_verifier) => token endpoint
1. hash code_verifier <==> check if it matches the stored code_challenge
1. id_token, access_token <== return
1. token validation => userinfo request (access_token) => userinfo endpoint
1. access token is validated
1. user claims returned


### Inspecting an Identity Token
Identity tokens are JWTs (Json Web Token)
```json
{ 
"sub": "b7539694-97e7-4dfe-84da-b4256e1ff5c7",
"given_name": "Emma",
"iss": "https://localhost:5001",
"aud": "imagegalleryclient",
"iat": 1490970940,
"exp": 1490971240,
"nbf": 1490970940,
"auth_time": 1490970937,
"amr": ["pwd"], 
"nonce": "63…200.ZjMzZ…5YzFlNWNiN2Mw…AtNGYyZi00MzYzNmZh", 
"at_hash": "90V_c-PO0kdoP-IOERlkdi"
…
}
```
- Subject: the user’s identifier
- Optional user claims related to the requested scopes
- Issuer: the issuer of the identity token
- Audience: the intended audience for this token
- Issued at: the time at which the JWT was issued
- Expiration: the expiration time on or after which the identity token must not be accepted for processing
- Not before: the time before which the identity token must not be accepted for processing
- Authentication time: the time of the original authentication
- Authentication methods references: identifiers for authentication methods
- Number only to be used once
- Access token hash: Base64 encoded value of the left-most half of the hash of the octets of the ASCII representation of the access token

### ClaimsIdentity

- ClaimsIdentity is created from a validated id_token
- Claims can be returned from the UserInfo endpoint to avoid issues with URL length 
- restrictions & decrease the gains of a potential attack
- When logging out, remember to log out of the IDP if required






