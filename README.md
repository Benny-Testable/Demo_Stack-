# Scholarship Awards Platform

Applicant intake, eligibility scoring and award management for a
multi-programme scholarship trust.

## Stack

| Layer | Technology |
|-------|------------|
| Frontend | ReactJS (JavaScript) |
| Backend | ASP.NET Core 8 MVC |
| Database | SQL Server |

## Architecture

Entire MVC architecture. Controllers bind requests and coordinate, Models
carry the domain and persistence shape, Views render server-side Razor pages.
The React client consumes the same controllers over JSON.

```
backend/src/Awards.Web
  Controllers/   request handling
  Models/        domain entities and view models
  Views/         Razor views
  Data/          EF Core context, seed data
  Services/      eligibility scoring, statements, sanitisation
backend/tests     xUnit test suite
frontend          ReactJS client
```

## Running

```
dotnet restore backend/Awards.sln
dotnet run --project backend/src/Awards.Web
npm install --prefix frontend
npm run dev --prefix frontend
```

## Tests

```
dotnet test backend/Awards.sln
npm test --prefix frontend
```
