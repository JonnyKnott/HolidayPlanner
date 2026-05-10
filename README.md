# HolidayPlanner

Holiday planning application with a .NET 9 backend API and React/TypeScript web frontend (React Native / PWA mobile planned).

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

## Getting started

**Start local dependencies (SQL Server + MongoDB):**
```bash
docker compose up -d
```

**Run the API:**
```bash
cd src/HolidayPlanner.Api
dotnet run
```
API will be available at `https://localhost:5001`. Swagger UI at `https://localhost:5001/swagger`.

**Run unit tests:**
```bash
dotnet test tests/HolidayPlanner.Tests
```

**Run integration tests** (requires `docker compose up -d`):
```bash
dotnet test tests/HolidayPlanner.IntegrationTests
```

## Solution structure

| Project | Purpose |
|---|---|
| `HolidayPlanner.Domain` | Domain models, aggregates, value objects, domain events |
| `HolidayPlanner.Application` | CQRS commands/queries, MediatR handlers, FluentValidation |
| `HolidayPlanner.Infrastructure` | EF Core (SQL Server), MongoDB, repositories |
| `HolidayPlanner.Api` | ASP.NET Core Web API, controllers, middleware |
| `HolidayPlanner.Tests` | xUnit unit tests |
| `HolidayPlanner.IntegrationTests` | xUnit integration tests against real infrastructure |
