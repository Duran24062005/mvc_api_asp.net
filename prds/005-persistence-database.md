# PRD 005 - Persistence and Database

## 1. Problem and Goal

- Problem: The current API keeps data in memory, which is useful for learning but does not survive application restarts.
- Goal: Document the current storage model and define the target database shape for a future persistent implementation.

## 2. Scope

- Included: Current in-memory persistence, entities, seed data, normalization rules, and proposed relational schema.
- Excluded: Implementing Entity Framework Core, creating migrations, choosing a database provider, and deploying a database.

## 3. Affected Actors

- Actor: API developer.
- Responsibility or access: Understands where data lives and how to migrate it later.
- Actor: API consumer.
- Responsibility or access: Receives stable contracts that do not expose internal persistence details.

## 4. Functional Requirements

- Requirement: Document current persistence behavior.
- Expected behavior: Developers can see that data resets on restart and lives in singleton repositories.
- Requirement: Document data entities and fields.
- Expected behavior: Developers can map API DTOs to internal records and future tables.
- Requirement: Document future database direction.
- Expected behavior: A future EF Core migration can start from the documented schema.

## 5. API Contract

- Endpoint: Not applicable.
- Method: Not applicable.
- Auth: Not applicable.
- Request: Not applicable.
- Response: Not applicable.

## 6. Data and Rules

- Data model: `UserAccount`, `Movie`, and `Customer`.
- Validation: Validation currently lives in request DTOs and controller model binding.
- Business rules: Customer email is normalized to lowercase; password hashes are never returned.

## 7. Permissions

- Public access: None.
- Authenticated access: No direct database access.
- Role-based access: Access is enforced at API endpoint level, not repository level.

## 8. Implementation Approach

- Application layer: Keep controllers and services independent from the concrete repository implementation.
- Infrastructure or persistence: Current implementation uses in-memory repositories; future implementation can replace them with EF Core repositories behind the same interfaces.
- Error handling: Repository misses map to `404 Not Found` in controllers.

## 9. Risks and Edge Cases

- Risk: Developers may assume the current data survives restarts.
- Mitigation: Document in-memory behavior clearly in `docs/SystemArtifact.md`.
- Risk: A future database may drift from current DTO validation.
- Mitigation: Mirror key validations in database constraints when persistence is implemented.

## 10. Validation Plan

- Automated tests: Keep integration tests green when repository implementations change.
- Manual checks: Restart the app and confirm in-memory records reset until a real database is implemented.
