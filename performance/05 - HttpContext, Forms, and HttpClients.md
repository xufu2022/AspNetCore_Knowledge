# HttpContext, Forms, and HttpClients

## Module Theme : Avoid Traps!

- Bait: Something that seems easy enough
- Trap: Unpredictable behavior, thread pool starvation, more
- Focus on demos in our application code:
    - HttpContext and IHttpContextAccessor
    - Forms and Request body
    - HttpClient – making API calls

## Wait –what??

HttpContext, Forms, HttpClient??
I’ve already got working code – and what does this have to do with performance??
This is like the async module: the code you have that works may not perform well under load

- Improper use of HttpContext can result in hangs, app crashes, or data corruption
- HttpContext: Available in Controllers and PageModels
- IHttpContextAccessor for other classes
  - Don’t capture HttpContext
- HttpContext is not thread-safe
  - Watch out for parallel task execution

## For forms and body content, use async methods or framework features

## HttpClient

First rule: don’t create them for each usage!

Multiple ways to use IHttpClientFactory correctly:
- Injected HttpClient (more duplicated code)
- Named clients
- Typed clients