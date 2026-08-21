# PRD 006 - Modular Monolith

## 1. Problem and Goal

- Problem: The API is organized by technical layer, so related behavior for each business area is spread across controllers, contracts, domain records, mappings, services, and repositories.
- Goal: Restructure the single deployable API into a modular monolith where each business capability owns its HTTP surface, contracts, domain model, application ports, and in-memory infrastructure.

## 2. Scope

- Included: Folder and namespace reorganization, module-level dependency injection registration, documentation updates, and test namespace updates.
- Excluded: Splitting into multiple deployables, adding a database, adding message buses, introducing background workers, and changing public HTTP routes or payload shapes.

## 3. Affected Actors

- Actor: API developer.
- Responsibility or access: Works inside a clear module boundary when changing identity, movies, or customers.
- Actor: API consumer.
- Responsibility or access: Continues using the same HTTP routes and JSON contracts.
- Actor: Maintainer.
- Responsibility or access: Uses module composition methods to see which services belong to each capability.

## 4. Functional Requirements

- Requirement: Preserve current API behavior.
- Expected behavior: Existing authentication, authorization, movie CRUD, customer CRUD, and user endpoints keep the same routes, status codes, and response payloads.
- Requirement: Make module ownership explicit.
- Expected behavior: Identity, Movies, and Customers each contain their own contracts, domain records, mappings, controllers, repositories, and DI registration.
- Requirement: Keep a single application boundary.
- Expected behavior: The system still builds and runs as one ASP.NET Core Web API project and one process.
- Requirement: Isolate shared concepts.
- Expected behavior: Cross-module roles live in a small shared kernel instead of being owned by one feature module.

## 5. API Contract

- Endpoint: Existing `/api/auth`, `/api/users`, `/api/movies`, and `/api/customers` routes.
- Method: Existing methods are unchanged.
- Auth: Existing JWT Bearer and role requirements are unchanged.
- Request: Existing request DTO shapes are unchanged.
- Response: Existing response DTO shapes are unchanged.

## 6. Data and Rules

- Data model: `UserAccount`, `Movie`, and `Customer` remain the internal records for their modules.
- Validation: Existing data annotations and custom validation rules remain on request DTOs.
- Business rules: Existing normalization rules remain in the in-memory repositories.
- Persistence: Current in-memory repositories remain the implementation for this learning version.

## 7. Permissions

- Public access: `POST /api/auth/login` remains public.
- Authenticated access: `/api/users/me`, movies, and customers remain protected.
- Role-based access: `/api/users` remains `Admin` only; movies and customers remain `Admin` or `User`.

## 8. Implementation Approach

- Application layer: Each module owns its public service/repository ports used by its controllers or services.
- Infrastructure or persistence: Each module owns its in-memory repository implementation and related technical services.
- Composition: `Program.cs` calls module registration methods such as `AddIdentityModule`, `AddMoviesModule`, and `AddCustomersModule`.
- Shared kernel: Authorization role names live under `SharedKernel` because multiple modules need them.
- Error handling: Existing controller-level status code behavior remains unchanged.

## 9. Risks and Edge Cases

- Risk: Folder movement can break namespace imports without changing runtime behavior.
- Mitigation: Update all references and run integration tests after the restructure.
- Risk: A module can start depending directly on another module's internals.
- Mitigation: Keep shared concepts small and document the expected ownership boundaries.
- Risk: Future persistence work may accidentally centralize all storage again.
- Mitigation: Keep repository interfaces and implementations inside the owning module.

## 10. Validation Plan

- Automated tests: Run the existing integration test suite after the restructure.
- Manual checks: Start the API, log in, and call protected endpoints if runtime verification is needed beyond tests.
