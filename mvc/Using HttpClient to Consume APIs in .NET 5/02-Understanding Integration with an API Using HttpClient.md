# Understanding Integration with an API Using HttpClient

```csharp

var httpClient = new HttpClient();
var response = await _httpClient.GetAsync("http://localhost:123/api/movies");

response.EnsureSuccessStatusCode();

var content = await response.Content.ReadAsStringAsync();
var movies = JsonSerializer.Deserialize<List<Movie>>(content);

```
## Tackling Integration with HttpClient

Http is a request-response protocol between a client and server
A browser is an Http client that can send messages and capture responses


Message Handler (: HttpMessageHandler)

1. HttpRequestMessage
1. API
1. HttpResponseMessage
1. HttpClient
1. HttpRequestMessage

> Sharing DTO classes
- Linked files
- Shared assembly 
> Generating DTO classes
- Start from an OpenAPI specification

Each HttpRequestMessage travels through a set of handlers, and the HttpResponseMessage travels back up through the same set
- Handlers can pass on requests or cancel them
- HttpClientHandler is responsible for sending the actual request

