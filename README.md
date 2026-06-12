# Medora Health Tech

A scalable hospital operations and HR operations management platform built with .NET 8, Angular, SQL Server, and Entity Framework Core.

## Solution Structure

- `src/Medora.Api` - ASP.NET Core Web API host, controllers, auth, CORS, health checks.
- `src/Medora.Application` - use-case/application abstractions.
- `src/Medora.Domain` - core domain entities and business concepts.
- `src/Medora.Infrastructure` - EF Core SQL Server persistence, seed data, external services.
- `tests/Medora.Tests` - automated tests.

## Backend Foundation

The initial backend setup includes:

- .NET 8 pinned via `global.json`.
- Clean architecture style project separation.
- SQL Server connection via EF Core.
- Tenant-scoped domain entities.
- Role and permission seed catalog for hospital operations and HR operations.
- Idempotent database seeder for tenants, roles, permissions, hospital operations data, and HR operations data.
- Health check endpoint at `/health`.
- Setup catalog endpoint at `/api/setup/catalog`.
- Development seed endpoint at `/api/setup/seed`.
- Database-backed screen catalog endpoint at `/api/operations/screens`.
- Tenant-scoped operational APIs under `/api/operations/*`.
- Tenant-scoped HR APIs under `/api/operations/hr/*`.

## Database

Default local connection string:

```text
Server=localhost;Database=MedoraHealthTech;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true
```

Startup database mutation is disabled by default:

```json
"Database": {
  "ApplyMigrationsOnStartup": false,
  "SeedOnStartup": false
}
```

Enable only for controlled local development or deployment automation.

## Commands

```powershell
dotnet restore
dotnet build
dotnet test
```

Seed roles and permissions after the database exists:

```http
POST /api/setup/seed
```

The seed operation now also creates the St. Mary, Riverside, and Northgate tenants, plus the initial module, clinical, finance, audit, and HR records used by the Angular screens.

## Architecture Direction

Start as a modular monolith. Keep module boundaries explicit in domain/application code, enforce RBAC and tenant scope server-side, and split into services later only when deployment or scale pressure justifies it.

## EF Core Migrations

The repo uses a local `dotnet-ef` tool manifest.

```powershell
dotnet tool restore
dotnet tool run dotnet-ef migrations add <MigrationName> --project src\Medora.Infrastructure\Medora.Infrastructure.csproj --startup-project src\Medora.Api\Medora.Api.csproj --output-dir Persistence\Migrations
dotnet tool run dotnet-ef database update --project src\Medora.Infrastructure\Medora.Infrastructure.csproj --startup-project src\Medora.Api\Medora.Api.csproj
```

After applying the database, seed default roles and permissions with:

```http
POST /api/setup/seed
```

For controlled local development only, you may temporarily set:

```json
"Database": {
  "ApplyMigrationsOnStartup": true,
  "SeedOnStartup": true
}
```

Do not leave automatic migration/seeding enabled in production unless it is part of a reviewed deployment process.

## Frontend

Angular app location:

```text
frontend/medora-web
```

Common commands:

```powershell
cd frontend\medora-web
npm install
npm run build
npm start
```
