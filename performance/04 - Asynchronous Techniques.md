# Asynchronous Techniques

## Why Async?

One at a time, 

please Synchronous code means everyone waits their turn in order

Handoff to others

Asynchronous code keeps the main thread very open while handing operations off to other threads

**Asynchronous code enables more concurrency, not speed**

- Update some synchronous code to be asynchronous
   - Prefer async/await to alternatives
   - Notice how it propagates
- Can’t always use async/await
- Orchestration of parallel activity

## Cancellation Tokens

Useful for stopping heavy operations

Read operations are easy

Update operations require care

## Summary

- Async code can enable more concurrency
- Async code propagates quickly
- Async/await cannot always be used
- Parallel execution does help speed
- Cancellation tokens avoid unnecessary processing