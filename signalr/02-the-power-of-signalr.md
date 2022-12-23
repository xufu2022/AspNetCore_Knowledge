# Real-time Web Applications

## Hub and Clients

- Hub can be part of any ASP.NET Core server-side application
- Demo: MVC application with views and hub
- Client-side code in javascript
- Clients call methods on the hub and respond to hub calls
- Clients can also be .NET or Java applications

## Remote Procedure Call Method Function

Remote Procedure Call (RPC)

Browser (client) => Hub (server)

```c#
NotifyNewBid(AuctionNotify auction) 
{ 
    Clients.All.SendMessage("ReceiveNewBid", auction);
}
```

## Hub Protocol

> JSON Hub Protocol

```json
{
"type":1,
"target":"ReceiveNewBid",
"arguments":[
    {"auctionId":1,
    "newBid":25
    }]
}
```

## Coming Up

- Write the client-side part in javascript:
- Establish connection
- Call NotifyNewBid
- Write ReceiveNewBid

## Transports

- WebSockets
- Server Sent Events (SSE)
- Long Polling

Only transport that offers a true 2-way, full-duplex connection between client and server that stays open.
Upgrades the socket of a normal HTTP request to a websocket.
The connection stays until closed or a network problem occurs.

## Server Sent Events

- Uses HTTP requests
- Can do server to client HTTP requests
- Uses HTTP requests for each message from client to server

## Long Polling

- For server to client messages the client does an HTTP request to the server which remains open.
- Until there is a message to send or a request timeout occurs.
- Rinse and repeat.

## Transport Negotiation

- Websockets have to be supported throughout the network chain.
- If not SignalR will fall back to server sent events or long polling.
- Need for other transports is less nowadays.
- Consider disabling negotiation and fall back.