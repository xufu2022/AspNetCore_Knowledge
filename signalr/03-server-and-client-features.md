# Server and Client Features

## Clients

- Can be of any application type
- For example: desktop, web, mobile or console
- Don't all have to be of the same type
- Can use any technology
- There are client libraries for javascript, .NET and Java

## IHubContext

- Can be used anywhere in the application
- Doesn't have the concept of a "caller"

## Client Groups

- Groups of clients
- Use connection id
- Heads up: connection ids change
- Dynamic, on the fly
- Functions can be called on groups

## Hub Protocol

- JSON might not be efficient enough
- Especially when the messages are large
- Option: MessagePack, a binary hub protocol

**@microsoft/signalr-protocol-msgpack**

MessagePack serialization is case_sensitive

Be careful with turning on EnableDetailedErrors

## Design Considerations

- Fallback scenario if SignalR connection isn't available
- Combine SignalR with traditional API
- Use single page applications if application has more than one page
- Combine multiple simple type parameters in a complex object
- Consider automatic reconnections

## Streaming

Only suitable for specific scenarios
Messages in Websockets connection already relatively cheap
Not for audio/video
Lengthy calculations or processing

https://github.com/RolandGuijt/SignalRStreaming

