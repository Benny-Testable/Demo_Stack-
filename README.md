# Estates Portfolio Platform

Property and leasing management for a multi-site commercial portfolio.

## Stack

| Layer | Technology |
|-------|------------|
| Frontend | ReactJS (JavaScript) |
| Backend | .NET Core MVC (Model-View-Controller) |
| Database | SQL Server |

## Architecture

Entire MVC architecture. Controllers handle routing and request binding, Models
carry the domain and persistence shape, Views render server-side Razor pages.
The React frontend consumes the same controllers over JSON for the interactive
portfolio screens.

```
backend/src/Estates.Web
  Controllers/   request handling
  Models/        domain entities and view models
  Views/         Razor views
  Data/          EF Core context, migrations, seed data
  Services/      pricing, reporting, sanitisation
backend/tests     xUnit test suite
frontend          ReactJS client
```

## Running

```
dotnet restore backend/Estates.sln
dotnet run --project backend/src/Estates.Web
npm install --prefix frontend
npm run dev --prefix frontend
```

## Tests

```
dotnet test backend/Estates.sln
npm test --prefix frontend
```
