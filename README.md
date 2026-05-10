# HolidayPlanner

Holiday planning application with a .NET 9 backend API and React/TypeScript web frontend (React Native / PWA mobile planned).

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)

## Setup

**Docker dependencies (SQL Server + MongoDB):**

Copy the example environment file and set your SQL Server password. This `.env` file is used by docker-compose only — it is not read by the .NET application:
```bash
cp .env.example .env
```

Edit `.env` if you want to use a different password, then continue with the steps below.

**Supplying the database password to the .NET app:**

`appsettings.Development.json` intentionally omits the SQL Server password. Supply it at runtime using one of these approaches:

- Set an environment variable before running `dotnet run`:
  ```bash
  export ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=HolidayPlanner;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
  ```
  On Windows (PowerShell):
  ```powershell
  $env:ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=HolidayPlanner;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;"
  ```

- Or use .NET User Secrets (recommended for local development — never committed to source control):
  ```bash
  dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=localhost,1433;Database=HolidayPlanner;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True;" --project src/HolidayPlanner.Api
  ```

Replace `YourStrong@Passw0rd` with the value of `SA_PASSWORD` from your `.env` file.

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
API will be available at `https://localhost:7169`. Swagger UI at `https://localhost:7169/swagger`.

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
