# PRD 002 - Movies

## 1. Problem and Goal

- Problem: The project needs a concrete movie domain to practice CRUD APIs.
- Goal: Provide authenticated CRUD endpoints for movies with simple validation and predictable sample data.

## 2. Scope

- Included: List, detail, create, update, and delete movies.
- Excluded: Reviews, ratings aggregation, file uploads, streaming availability, database persistence.

## 3. Affected Actors

- Actor: Authenticated user.
- Responsibility or access: Can read and manage movie records in this learning API.

## 4. Functional Requirements

- Requirement: List all movies.
- Expected behavior: Returns seeded and newly created movies.
- Requirement: Get one movie by id.
- Expected behavior: Returns `404 Not Found` when the movie does not exist.
- Requirement: Create a movie.
- Expected behavior: Valid payload returns `201 Created` with the new movie.
- Requirement: Update a movie.
- Expected behavior: Valid payload replaces editable fields.
- Requirement: Delete a movie.
- Expected behavior: Existing movie deletion returns `204 No Content`.

## 5. API Contract

- Endpoint: `/api/movies`
- Method: `GET`
- Auth: Bearer token
- Request: None
- Response: `200 OK` with an array of movies
- Endpoint: `/api/movies/{id}`
- Method: `GET`
- Auth: Bearer token
- Request: Route `id`
- Response: `200 OK` or `404 Not Found`
- Endpoint: `/api/movies`
- Method: `POST`
- Auth: Bearer token
- Request: `{ title, genre, releaseYear, director }`
- Response: `201 Created`
- Endpoint: `/api/movies/{id}`
- Method: `PUT`
- Auth: Bearer token
- Request: `{ title, genre, releaseYear, director }`
- Response: `204 No Content` or `404 Not Found`
- Endpoint: `/api/movies/{id}`
- Method: `DELETE`
- Auth: Bearer token
- Request: Route `id`
- Response: `204 No Content` or `404 Not Found`

## 6. Data and Rules

- Data model: Movie has `id`, `title`, `genre`, `releaseYear`, `director`, `createdAtUtc`, and `updatedAtUtc`.
- Validation: Title, genre, and director are required; release year must be between 1888 and next calendar year.
- Business rules: IDs are generated server-side.

## 7. Permissions

- Public access: None.
- Authenticated access: All movie endpoints.
- Role-based access: `Admin` and `User`.

## 8. Implementation Approach

- Application layer: Controller delegates state changes to `IMovieRepository`.
- Infrastructure or persistence: Thread-safe in-memory repository for learning and tests.
- Error handling: Missing records return `404`; validation errors return standard ASP.NET Core validation responses.

## 9. Risks and Edge Cases

- Risk: In-memory data resets on app restart.
- Mitigation: Document persistence as intentionally temporary.
- Risk: Concurrent requests can mutate data.
- Mitigation: Use repository locking around shared lists.

## 10. Validation Plan

- Automated tests: Authenticated movie list returns seeded data; unauthenticated movie access returns `401`.
- Manual checks: Login, list movies, create a movie, fetch it by id, update it, then delete it.
