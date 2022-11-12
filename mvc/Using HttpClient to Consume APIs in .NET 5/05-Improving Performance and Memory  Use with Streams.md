# Improving Performance and Memory Use with Streams

- Advantages of working with streams
- Using streams when reading data
- Using streams when sending data
- Testing performance improvements
- Improving performance with compression

## Stream
An abstraction of a sequence of bytes, such as a file, an input/output device or network traffic


> Advantages of Working with Streams
- Classes derived from Stream hide specific details of the operating system and the underlying devices
- Streams help with avoiding large in-between variables
  - Better for memory use
  - Better for performance
- The API doesn’t need to work with streams to get these advantages at client level

> Using Streams When Reading Data
- in-memory string

1. GET api/movies/{movieId}/posters/{posterId}
1. API
1. wait for content to arrive
1. parse content as string
1. deserialize string into Poster
   
- stream

1. GET api/movies/{movieId}/posters/{posterId}
2. API
3. wait for content to arrive
4. stream
5. deserialize string into Poster

> Improving Memory Use and Performance with HttpCompletionMode
- Using Streams When Sending Data
1. POST api/movies/{movieId}/posters
1. create Poster object
1. serialize object to json
1. send complete content
1. API

- Using Streams When Sending Data
1. POST api/movies/{movieId}/posters
1. stream
1. send complete content
1. API
   
Combining streams when sending and reading data

## On Streaming, Memory Use and Performance
Creating and disposing streams can cause some overhead
- You may see a direct impact on performance

Using streams ensures memory use is kept low
Minimizing memory can also minimize garbage collection, which has a positive impact on performance

**Always** use streams when reading data
Also use streams when sending large amounts of data
If you’re not sure, test

# Summary

Streams are the preferred way of interacting with an API
- Reduced memory footprint
- Improved performance

Streams can be used both when reading and sending data
- Use HttpCompletionMode to start streaming the response once response headers have arrived
- Enable compression by setting the Accept-Encoding header & enabling automatic decompression on the HttpClientHandler instance