# Authentication and Authorization

## Cookie Authentication

Auctions application

- MVC/API endpoints receives and verifies cookie on each request
- SignalR with Websockets transport receives and verifies cookie only when connection is established

## Token-based Authentication

- Cookies only work in browsers
- Other types of clients can use tokens
- OpenID Connect/OAuth
- Centralized user management

For non-browser interactive clients use Authorization Code flow with PKCE
Need a way to show pages provided by identity provider
Browsers can't keep a secret. Don't let browsers get the token
Instead use BFF (Backend For Frontend)

## OpenID Connect with BFF

## Duende BFF

### Supporting Both Client Types

- With BFF setup console client can't connect
- Option: create a separate API protected with an access token
- That includes the SignalR setup
- Browser clients now need to store an access token and that's not safe
- Choose between browser clients only or non-browser clients only
