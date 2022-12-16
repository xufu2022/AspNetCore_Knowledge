# Optimizing Responses and UI Assets

## Improving Network Stewardship

> SSL for HTTP/2 and /3

Browsers require SSL for these

> Response Compression

Send wire content in zipped format

> Minification and Bundling

Shrink UI assets and minimize requests

## HTTP/2 and HTTP/3

> HTTP/2
- Header compression
- Multiplex connections
- Faster load times
> HTTP/3
- Faster connection setup
- Better transition between networks
> Browsers require SSL to use them

> HTTP/2 on by default with ASP.NET Core (>2.2)

## Response Compression

> Relies on the Accept-Encoding HTTP header
- gzip, br are most common
> Applicable to text-based resources
- html, css, js, json, xml
> Avoid further compression of alreadycompressed content
- png, jpg, etc
> Middleware available within framework

**Not so fast!**
Web servers like IIS, nginx, and Apache can do response compression (often better)

## Minification and Bundling

> Minification
- Shrinks css and javascript
- Takes only required parts
- Eliminates whitespace & comments
- Renames local vars

> Bundling

Merging two or more files into one Reduces number of requests