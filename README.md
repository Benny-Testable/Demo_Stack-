# Coastline Fitness Platform

Membership, class scheduling and attendance for a multi-site fitness club.

## Stack

| Layer | Technology |
|-------|------------|
| Frontend | React + Vite (TypeScript) |
| Backend | ASP.NET Core 8 |
| Database | PostgreSQL |

## Architecture

Clean Architecture. Dependencies point inwards only.

```
Domain          entities, value objects, domain rules. No dependencies.
Application     use cases, DTOs, abstractions. Depends on Domain.
Infrastructure  EF Core, PostgreSQL, repositories. Depends on Application.
Api             controllers, composition root. Depends on all three.
```

```
backend/src/Coastline.Domain
backend/src/Coastline.Application
backend/src/Coastline.Infrastructure
backend/src/Coastline.Api
backend/tests/Coastline.Tests
frontend
```

## Running

```
dotnet restore backend/Coastline.sln
dotnet run --project backend/src/Coastline.Api
npm install --prefix frontend
npm run dev --prefix frontend
```

## Tests

```
dotnet test backend/Coastline.sln
npm test --prefix frontend
```
