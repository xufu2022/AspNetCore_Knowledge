# Caching Techniques

## What to Cache

> Common “heavy” operation results 
-   Remote read operations, used frequently, high overhead

> Response content
-   Assets or content sent to browser, especially frequently-sent static content

## When to Cache

When you need to “Slow” operations frequently repeated with same results

NOTE: Adds complexity

## Caching Concepts

Key value pairs
Store key of what you’re requesting
Get from cache store instead of normal (slow) location

## Cache invalidation

Expiration-based
Explicit when source data changes

-   Application data caching
-   In-memory caching with expiration
-   Distributed caching with expiration
    - Serialization matters
    - Explicit cache invalidation

## In-Memory Caching

> Easy!

Very simple and built-in Fast! (no network hops)

> Be careful!

Easy to consume resources on server
Cache invalidation is hard (not distributed)

## Redis Options (IDistributedCache)

- Container using Docker Desktop (recommended) (WSL + Docker Desktop)

- Local install – still requires WSL

- Cloud service – pick your cloud

- Other implementations of IDistributedCache – SQL Server, NCache, even SQLite!

## Expiration-Based Caching: The Challenge

> Retrieve some data, then cache it

> What if something about the data changes

> Cache with expiration
  - Cached data will remain in use for cache duration
  - No updates will be seen

## Choosing a Distributed Cache

- Did you say SQL Server??
- Look-up is indexed, cached content is lowervolume
- Traffic not reduced
- Operation overhead should be reduced
- Persistence is optional

## Response Caching

Browser => Headers => Remote Web Location => Data Sources


## Response Caching Restrictions

- Must be GET or HEAD requests
- Cannot have an Authorization header
- Not for server-side UI apps (Razor Pages, MVC)
- What’s left??
    - Anonymous API calls
    - Static HTTP assets

## Demo:

Response caching with built-in middleware
Update API to be anonymous
Goal: avoid hitting API at all
Look at different query strings

## More Response Caching

- Microsoft Docs are a good resource: https://bit.ly/aspnetcoreresponse-caching
- Tag helpers (<cache> and <distributed-cache>) can be used in Razor syntax
- More HTTP caching header details here: https://mzl.la/3SXGnaI