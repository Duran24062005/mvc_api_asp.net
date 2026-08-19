<div align="center">
    <h1>MVC Movies API with .NET</h1>
</div>

Learning API built with ASP.NET Core Web API and .NET 10.

## Features

- JWT login with seeded `admin` and `user` accounts.
- Protected movie CRUD endpoints.
- Protected customer CRUD endpoints.
- Current-user endpoint and admin-only user listing.
- In-memory repositories for easy learning.
- PRDs in `prds/` and system documentation in `docs/SystemArtifact.md`.
- Integration tests with xUnit.

## Run

```bash
dotnet restore tests/MovieApi.Tests/MovieApi.Tests.csproj
dotnet build MoviesLearningApi.sln -m:1
dotnet run --project src/MovieApi/MovieApi.csproj --launch-profile http
```

Default URL:

```text
http://localhost:5075
```

## Seeded Users

| Username | Password | Role |
| --- | --- | --- |
| `admin` | `Admin123!` | `Admin` |
| `user` | `User123!` | `User` |

## Test

```bash
dotnet test tests/MovieApi.Tests/MovieApi.Tests.csproj
```

The test suite currently covers authentication, authorization, movie CRUD, customer CRUD, and user-profile access. The detailed test matrix is documented in `docs/SystemArtifact.md`.

## Data Storage

The current version uses in-memory repositories, so data resets when the API restarts. The current entities, seed data, rules, and future relational schema are documented in `docs/SystemArtifact.md`.

See `src/MovieApi/MovieApi.http` for request examples.
