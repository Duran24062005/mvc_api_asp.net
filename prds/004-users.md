# PRD 004 - Users

## 1. Problem and Goal

- Problem: The API needs user endpoints to show authenticated identity and admin-only access control.
- Goal: Provide endpoints for current-user profile and admin user listing.

## 2. Scope

- Included: Current user profile endpoint, admin-only list users endpoint, seeded users.
- Excluded: Registration, user creation, password update, account activation, persistent identity store.

## 3. Affected Actors

- Actor: Authenticated user.
- Responsibility or access: Can view their own profile.
- Actor: Admin user.
- Responsibility or access: Can list all users.

## 4. Functional Requirements

- Requirement: Get current profile.
- Expected behavior: Authenticated caller receives their own user profile from token claims.
- Requirement: List users as admin.
- Expected behavior: Admin receives all user profiles without password hashes.
- Requirement: Reject non-admin user listing.
- Expected behavior: Non-admin token receives `403 Forbidden`.

## 5. API Contract

- Endpoint: `/api/users/me`
- Method: `GET`
- Auth: Bearer token
- Request: None
- Response: `200 OK` with the current user profile
- Endpoint: `/api/users`
- Method: `GET`
- Auth: Bearer token with `Admin` role
- Request: None
- Response: `200 OK` with an array of user profiles

## 6. Data and Rules

- Data model: User has `id`, `username`, `displayName`, `email`, and `role`.
- Validation: Token must include `sub`, `name`, `email`, and `role` claims.
- Business rules: Password hashes are never returned by API responses.

## 7. Permissions

- Public access: None.
- Authenticated access: `/api/users/me`.
- Role-based access: `/api/users` requires `Admin`.

## 8. Implementation Approach

- Application layer: `UsersController` reads current identity claims and repository data.
- Infrastructure or persistence: `IUserRepository` exposes safe user profiles and credential validation.
- Error handling: Missing or inconsistent claims return `401 Unauthorized`; insufficient role returns `403 Forbidden`.

## 9. Risks and Edge Cases

- Risk: Claim names can be mapped unexpectedly by JWT middleware.
- Mitigation: Disable inbound claim mapping and use explicit JWT registered claim names.
- Risk: Password hashes might leak through DTOs.
- Mitigation: Separate internal `UserAccount` model from public `UserResponse`.

## 10. Validation Plan

- Automated tests: `/api/users/me` returns the caller profile; `/api/users` allows admin and forbids regular user.
- Manual checks: Login as admin and regular user, then call both endpoints.
