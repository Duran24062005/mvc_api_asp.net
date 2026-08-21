# System Artifact - Movie API

## 1. Purpose

This repository contains a learning API for movies, customers, and users built with ASP.NET Core Web API on .NET 10.

The project is intentionally documented and structured as a modular monolith so each business capability can be studied independently while still running as one API:

- Authentication: login with seeded users and JWT Bearer tokens.
- Movies: protected CRUD endpoints for a movie catalog.
- Customers: protected CRUD endpoints for client/customer records.
- Users: protected identity endpoints, including an admin-only user list.

## 2. Technology Stack

- Runtime: .NET 10
- Framework: ASP.NET Core Web API with controllers
- Authentication: JWT Bearer using `Microsoft.AspNetCore.Authentication.JwtBearer`
- API description: built-in ASP.NET Core OpenAPI JSON plus Swagger UI via `Swashbuckle.AspNetCore`
- Tests: xUnit with `Microsoft.AspNetCore.Mvc.Testing`
- Persistence: in-memory repositories for the current learning version

## 3. Repository Layout

```text
.
├── docs/
│   └── SystemArtifact.md
├── prds/
│   ├── 000-doc-template.md
│   ├── 001-authentication.md
│   ├── 002-movies.md
│   ├── 003-customers.md
│   ├── 004-users.md
│   ├── 005-persistence-database.md
│   └── 006-modular-monolith.md
├── src/
│   └── MovieApi/
│       ├── Modules/
│       │   ├── Customers/
│       │   │   ├── Application/
│       │   │   ├── Contracts/
│       │   │   ├── Domain/
│       │   │   ├── Infrastructure/
│       │   │   ├── Mapping/
│       │   │   └── Presentation/
│       │   ├── Identity/
│       │   │   ├── Application/
│       │   │   ├── Configuration/
│       │   │   ├── Contracts/
│       │   │   ├── Domain/
│       │   │   ├── Infrastructure/
│       │   │   ├── Mapping/
│       │   │   └── Presentation/
│       │   └── Movies/
│       │       ├── Application/
│       │       ├── Contracts/
│       │       ├── Domain/
│       │       ├── Infrastructure/
│       │       ├── Mapping/
│       │       └── Presentation/
│       ├── OpenApi/
│       ├── SharedKernel/
│       ├── Program.cs
│       └── appsettings.json
├── tests/
│   └── MovieApi.Tests/
└── MoviesLearningApi.sln
```

## 4. Application Architecture

The application is a modular monolith:

- One ASP.NET Core project, one process, and one deployable API.
- Business capabilities are grouped under `Modules/`.
- Each module owns its HTTP controllers, API contracts, domain records, application ports, mappers, and infrastructure implementations.
- `Program.cs` is the composition root. It configures cross-cutting ASP.NET Core features and calls module registration methods.
- `SharedKernel` contains concepts intentionally shared by more than one module. It currently contains authorization role names.
- `OpenApi` remains cross-cutting because it configures the API document rather than one business capability.

### Module Boundaries

| Module | Owns | Public HTTP surface |
| --- | --- | --- |
| `Identity` | Login, JWT token creation, seeded users, password hashing, user profile mapping, user repository port and in-memory implementation | `/api/auth`, `/api/users` |
| `Movies` | Movie contracts, movie domain record, movie mapper, movie repository port and in-memory implementation | `/api/movies` |
| `Customers` | Customer contracts, customer domain record, customer mapper, customer repository port and in-memory implementation | `/api/customers` |

This is not a production architecture yet. It keeps the learning API small while making ownership boundaries visible.

## 5. Database and Persistence

The current project does not use an external database engine yet. Persistence is implemented with singleton in-memory repositories registered in dependency injection:

- `Modules/Identity/Infrastructure/InMemoryUserRepository`
- `Modules/Movies/Infrastructure/InMemoryMovieRepository`
- `Modules/Customers/Infrastructure/InMemoryCustomerRepository`

This means data lives only while the application process is running. Restarting the API resets movies, customers, and users to the seeded records.

### Current Storage Ownership

| Repository | Entity | Responsibility |
| --- | --- | --- |
| `IUserRepository` | `UserAccount` | Stores seeded users, validates credentials, exposes safe user records |
| `IMovieRepository` | `Movie` | Stores movie catalog records |
| `ICustomerRepository` | `Customer` | Stores customer records |

### Current Data Model

`UserAccount`

| Field | Type | Notes |
| --- | --- | --- |
| `Id` | `Guid` | Server-defined user identifier |
| `Username` | `string` | Login name, seeded as lowercase |
| `DisplayName` | `string` | Human-readable name |
| `Email` | `string` | User email |
| `Role` | `string` | `Admin` or `User` |
| `PasswordHash` | `string` | PBKDF2 hash; never returned by API responses |

`Movie`

| Field | Type | Notes |
| --- | --- | --- |
| `Id` | `Guid` | Server-generated identifier |
| `Title` | `string` | Required |
| `Genre` | `string` | Required |
| `ReleaseYear` | `int` | Must be between 1888 and next calendar year |
| `Director` | `string` | Required |
| `CreatedAtUtc` | `DateTimeOffset` | Set on create |
| `UpdatedAtUtc` | `DateTimeOffset` | Updated on edit |

`Customer`

| Field | Type | Notes |
| --- | --- | --- |
| `Id` | `Guid` | Server-generated identifier |
| `FullName` | `string` | Required |
| `Email` | `string` | Required and normalized to lowercase |
| `PhoneNumber` | `string?` | Optional |
| `CreatedAtUtc` | `DateTimeOffset` | Set on create |
| `UpdatedAtUtc` | `DateTimeOffset` | Updated on edit |

### Seed Data

Users:

| Id | Username | Role |
| --- | --- | --- |
| `cccccccc-cccc-cccc-cccc-ccccccccccc1` | `admin` | `Admin` |
| `cccccccc-cccc-cccc-cccc-ccccccccccc2` | `user` | `User` |

Movies:

| Id | Title | Release year |
| --- | --- | --- |
| `bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb1` | `The Matrix` | `1999` |
| `bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbb2` | `Inception` | `2010` |

Customers:

| Id | Full name | Email |
| --- | --- | --- |
| `aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1` | `Maria Gomez` | `maria.gomez@example.com` |
| `aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa2` | `John Carter` | `john.carter@example.com` |

### Data Rules

- IDs are generated by the API for new movies and customers.
- Movies and customers are stored in process memory.
- Movie text fields are trimmed before storage.
- Customer names are trimmed.
- Customer emails are trimmed and converted to lowercase.
- Empty customer phone numbers are stored as `null`.
- Password hashes are stored only in the internal `UserAccount` model.
- Public user responses use `UserResponse`, which excludes password hashes.

### Future Relational Schema

When the project moves from in-memory repositories to a database, the first relational model can map directly to the current entities:

```sql
CREATE TABLE users (
    id uuid PRIMARY KEY,
    username varchar(80) NOT NULL UNIQUE,
    display_name varchar(160) NOT NULL,
    email varchar(320) NOT NULL UNIQUE,
    role varchar(40) NOT NULL,
    password_hash text NOT NULL,
    created_at_utc timestamptz NOT NULL,
    updated_at_utc timestamptz NOT NULL
);

CREATE TABLE movies (
    id uuid PRIMARY KEY,
    title varchar(200) NOT NULL,
    genre varchar(80) NOT NULL,
    release_year int NOT NULL,
    director varchar(160) NOT NULL,
    created_at_utc timestamptz NOT NULL,
    updated_at_utc timestamptz NOT NULL,
    CONSTRAINT ck_movies_release_year CHECK (release_year >= 1888)
);

CREATE TABLE customers (
    id uuid PRIMARY KEY,
    full_name varchar(160) NOT NULL,
    email varchar(320) NOT NULL UNIQUE,
    phone_number varchar(40),
    created_at_utc timestamptz NOT NULL,
    updated_at_utc timestamptz NOT NULL
);
```

Recommended next implementation step for a real database:

- Add Entity Framework Core.
- Create `MovieDbContext`.
- Replace in-memory repositories with EF Core repositories.
- Add migrations for `users`, `movies`, and `customers`.
- Move seed data into migrations or a controlled startup seeding service.
- Move JWT signing keys and database connection strings to environment-specific secrets.

## 6. Authentication and Authorization

Authentication uses JWT Bearer tokens.

The application registers authentication in `Program.cs`; identity-specific services are registered by `AddIdentityModule`. Token validation checks:

- issuer
- audience
- signing key
- token lifetime
- role claim

JWT settings live in `src/MovieApi/appsettings.json` under `Jwt`.

```json
{
  "Jwt": {
    "Issuer": "MovieApi",
    "Audience": "MovieApi.Clients",
    "SigningKey": "development-only-movie-api-signing-key-change-this-before-production",
    "ExpirationMinutes": 60
  }
}
```

The signing key is development-only. In a real system, this value must come from environment-specific secret storage.

## 7. Seeded Users

The in-memory user repository seeds two users:

| Username | Password | Role |
| --- | --- | --- |
| `admin` | `Admin123!` | `Admin` |
| `user` | `User123!` | `User` |

Passwords are hashed with PBKDF2 before being stored in memory. This is still not a full identity system; it is a learning implementation.

## 8. Roles and Access Rules

| Area | Endpoint | Access |
| --- | --- | --- |
| Auth | `POST /api/auth/login` | Public |
| Movies | `/api/movies` | `Admin` or `User` |
| Customers | `/api/customers` | `Admin` or `User` |
| Users | `GET /api/users/me` | Any authenticated user |
| Users | `GET /api/users` | `Admin` only |

Unauthenticated requests return `401 Unauthorized`.
Authenticated users without the required role return `403 Forbidden`.

## 9. API Endpoints

### Auth

`POST /api/auth/login`

Request:

```json
{
  "username": "admin",
  "password": "Admin123!"
}
```

Successful response:

```json
{
  "accessToken": "jwt-token",
  "expiresAtUtc": "2026-08-19T00:00:00Z",
  "user": {
    "id": "cccccccc-cccc-cccc-cccc-ccccccccccc1",
    "username": "admin",
    "displayName": "Learning Admin",
    "email": "admin@example.com",
    "role": "Admin"
  }
}
```

### Movies

Protected by `Admin` or `User`.

| Method | Route | Behavior |
| --- | --- | --- |
| `GET` | `/api/movies` | List movies |
| `GET` | `/api/movies/{id}` | Get one movie |
| `POST` | `/api/movies` | Create movie |
| `PUT` | `/api/movies/{id}` | Update movie |
| `DELETE` | `/api/movies/{id}` | Delete movie |

Movie request:

```json
{
  "title": "Interstellar",
  "genre": "Science Fiction",
  "releaseYear": 2014,
  "director": "Christopher Nolan"
}
```

Validation:

- `title`, `genre`, and `director` are required.
- `releaseYear` must be at least 1888.
- `releaseYear` cannot be greater than next calendar year.

### Customers

Protected by `Admin` or `User`.

| Method | Route | Behavior |
| --- | --- | --- |
| `GET` | `/api/customers` | List customers |
| `GET` | `/api/customers/{id}` | Get one customer |
| `POST` | `/api/customers` | Create customer |
| `PUT` | `/api/customers/{id}` | Update customer |
| `DELETE` | `/api/customers/{id}` | Delete customer |

Customer request:

```json
{
  "fullName": "Maria Gomez",
  "email": "maria.gomez@example.com",
  "phoneNumber": "+57 300 000 0001"
}
```

Validation:

- `fullName` is required.
- `email` is required and must be valid.
- `phoneNumber` is optional.
- Emails are stored trimmed and lowercase.

### Users

`GET /api/users/me`

- Auth: any authenticated user.
- Returns the profile for the caller.

`GET /api/users`

- Auth: `Admin` only.
- Returns all user profiles.
- Never returns password hashes.

## 10. OpenAPI

In development, the API exposes Swagger UI at:

```text
/swagger
```

The API also exposes the built-in OpenAPI JSON document at:

```text
/openapi/v1.json
```

Swagger UI also exposes its generated JSON at:

```text
/swagger/v1/swagger.json
```

The OpenAPI documents include a Bearer security scheme so clients can understand that protected endpoints expect:

```text
Authorization: Bearer {token}
```

In Swagger UI, click `Authorize`, paste only the JWT token returned by `POST /api/auth/login`, and Swagger UI will send the `Authorization: Bearer {token}` header.

## 11. Local Development

Restore packages:

```bash
dotnet restore tests/MovieApi.Tests/MovieApi.Tests.csproj
```

Build:

```bash
dotnet build MoviesLearningApi.sln -m:1
```

Run the API:

```bash
dotnet run --project src/MovieApi/MovieApi.csproj --launch-profile http
```

The default local URL is:

```text
http://localhost:5075
```

Swagger UI:

```text
http://localhost:5075/swagger
```

Run tests:

```bash
dotnet test tests/MovieApi.Tests/MovieApi.Tests.csproj
```

The project-level restore command is documented because the current SDK/environment produced a silent failure when restoring the whole solution. The `-m:1` build flag is documented because the same environment produced a silent failure when building the whole solution in parallel. Individual project restores, sequential solution build, and tests pass.

## 12. Manual API Flow

1. Start the API with the `http` launch profile.
2. Send `POST /api/auth/login` using `admin` or `user`.
3. Copy `accessToken` from the response.
4. Send protected requests with this header:

```text
Authorization: Bearer {accessToken}
```

Example:

```bash
curl -X POST http://localhost:5075/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin123!"}'
```

```bash
curl http://localhost:5075/api/movies \
  -H "Authorization: Bearer {accessToken}"
```

## 13. Testing Strategy and Coverage

The test project uses xUnit and `Microsoft.AspNetCore.Mvc.Testing` to run integration tests against the real ASP.NET Core pipeline. These tests exercise routing, model binding, dependency injection, authentication middleware, authorization middleware, controllers, services, and in-memory repositories together.

Run tests with:

```bash
dotnet test tests/MovieApi.Tests/MovieApi.Tests.csproj
```

### Test Matrix

| Test | Scenario | Expected result |
| --- | --- | --- |
| `MoviesRequireAuthentication` | Anonymous caller requests `/api/movies` | `401 Unauthorized` |
| `LoginReturnsTokenAndAllowsMovieList` | Admin logs in and uses token on `/api/movies` | `200 OK` with seeded movies |
| `AuthenticatedUserCanCreateReadUpdateAndDeleteMovie` | Regular user completes movie CRUD | `201`, `200`, `204`, `204`, then `404` after delete |
| `AuthenticatedUserCanCreateReadUpdateAndDeleteCustomer` | Regular user completes customer CRUD | `201`, normalized email, `200`, `204`, `204`, then `404` after delete |
| `CurrentUserProfileReturnsCallerData` | Regular user calls `/api/users/me` | Response matches authenticated user |
| `RegularUserCannotListUsers` | Regular user calls admin-only `/api/users` | `403 Forbidden` |
| `AdminCanListUsers` | Admin calls `/api/users` | `200 OK` with seeded users |
| `InvalidLoginReturnsUnauthorized` | Caller submits wrong password | `401 Unauthorized` |

### Test Design Notes

- Tests use `WebApplicationFactory<Program>` so the full HTTP stack is exercised without starting Kestrel on a real port.
- Each test creates its own `HttpClient`.
- The application factory is shared by the test class, so repository state can be shared across tests. Current CRUD tests create their own records and delete them before finishing.
- Tests assert HTTP status codes and key response values instead of implementation details.
- Tests intentionally validate authorization behavior from the outside: missing token produces `401`; valid token with insufficient role produces `403`.

### Known Test Gaps

- No tests currently validate movie release-year boundary failures.
- No tests currently validate bad customer email payloads.
- No tests currently validate token expiration.
- No tests currently validate duplicate customer emails because uniqueness is not implemented yet.

## 14. Known Limitations and Future Work

- Data is in memory and resets when the application restarts.
- There is no database migration strategy yet.
- There is no user registration or password management flow.
- There are no refresh tokens.
- The JWT signing key is stored in development configuration.
- Customer email uniqueness is not enforced yet.
- Movie/customer writes are available to both `Admin` and `User`; future requirements may split read/write roles.
