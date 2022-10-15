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