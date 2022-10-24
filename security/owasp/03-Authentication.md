# Authentication
- ASP.NET Core Identity
- ASVS chapter 2 – “Authentication”
- Look at:
  - Passwords
  - Signup
  - Account confirmation
  - Password reset
  - Multi-factor authentication 
  - Rate limiting logins

Proving who you are
Username and password – credentials
Does someone else know your credentials?

### ASP.NET Core Identity
- Helps a lot with authentication
- ASVS has a lot of authentication requirements
  - A lot of code
  - Automated tests
  - Tried and tested patterns
  - Password storage etc
- Identity creates a lot of that code
  - Robust / well tested
  - Manage users
  - Security

### ASVS Authentication

> 2.1 Password Security
- Password format
- Workflows 
> 2.2 General Authenticator Security
- Passwords and authenticator apps
- Assume brute force / automation
- Avoid SMS and email
> 2.3 Authenticator Lifecycle
- Activation codes
- Suitably random?
> 2.5 Credential Recovery
- No password hints
- No secret questions
> 2.7 Out of Band Verifiers
- E.g. Push notifications
- Avoid SMS and email
> 2.8 One Time Verifiers
- PIN codes expire

### Passwords Are Secrets!
> Never store as plaintext
- No-one should be able to read passwords
- Not even database admins
> The fewer people who know a secret the better
