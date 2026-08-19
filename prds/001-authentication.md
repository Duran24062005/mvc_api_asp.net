# PRD 001 - Authentication

## 1. Problem and Goal

- Problem: The API needs a clear way to identify callers before granting access to protected resources.
- Goal: Provide a learning-friendly JWT authentication flow with seeded users and role claims.

## 2. Scope

- Included: Login endpoint, JWT token creation, token validation, role claims, seeded credentials for learning.
- Excluded: User registration, password reset, refresh tokens, external identity providers, database-backed identity.

## 3. Affected Actors

- Actor: Anonymous caller.
- Responsibility or access: Can call the login endpoint with valid credentials.
- Actor: Authenticated user.
- Responsibility or access: Can call endpoints allowed by their role.

## 4. Functional Requirements

- Requirement: Login with username and password.
- Expected behavior: A valid credential pair returns a signed JWT and basic user profile data.
- Requirement: Reject invalid credentials.
- Expected behavior: Invalid credentials return `401 Unauthorized` without exposing which field failed.
- Requirement: Include roles in tokens.
- Expected behavior: Authorization attributes can evaluate `Admin` and `User` role claims.

## 5. API Contract

- Endpoint: `/api/auth/login`
- Method: `POST`
- Auth: Public
- Request: `{ "username": "admin", "password": "Admin123!" }`
- Response: `200 OK` with `{ accessToken, expiresAtUtc, user }`

## 6. Data and Rules

- Data model: Users have `id`, `username`, `displayName`, `email`, `role`, and password hash.
- Validation: Username and password are required.
- Business rules: Tokens expire according to `Jwt:ExpirationMinutes`.

## 7. Permissions

- Public access: Login only.
- Authenticated access: Protected domain endpoints.
- Role-based access: `Admin` can manage users; `Admin` and `User` can access movies and customers.

## 8. Implementation Approach

- Application layer: `AuthService` validates credentials and delegates token creation.
- Infrastructure or persistence: `InMemoryUserRepository` stores seeded learning users.
- Error handling: Invalid credentials return `Unauthorized`; validation errors return model-state responses.

## 9. Risks and Edge Cases

- Risk: Hard-coded credentials are unsafe for production.
- Mitigation: Document this as a learning-only persistence strategy and isolate credentials in seed data.
- Risk: Weak JWT secret breaks token security.
- Mitigation: Provide a development secret and document production replacement through configuration.

## 10. Validation Plan

- Automated tests: Login succeeds with seeded admin; login rejects invalid password; protected endpoint rejects missing token.
- Manual checks: Use `/api/auth/login`, copy token, and call protected endpoints with `Authorization: Bearer <token>`.
