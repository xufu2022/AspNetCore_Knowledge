# Running Worker Services in Production

Overview

- Running workers in containers
- Running workers as Windows services
- Running workers as Linux daemons
- Running workers on Azure App Service

## Docker Primer

Containers

Containers are a popular choice for deploying microservices to production. Worker services can easily be deployed in containers.

## Key principles and terminology

## UseWindowsService Method

- Configures the host to use a WindowsServiceLifetime
- Sets the ContentRootPath to AppContext.BaseDirectory
- Enables logging to the Windows event log
