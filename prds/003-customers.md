# PRD 003 - Customers

## 1. Problem and Goal

- Problem: The API needs protected client/customer endpoints to practice access-controlled business resources.
- Goal: Provide authenticated CRUD endpoints for customers with simple contact data.

## 2. Scope

- Included: List, detail, create, update, and delete customers.
- Excluded: Billing, subscriptions, customer-movie rental history, external CRM integration.

## 3. Affected Actors

- Actor: Authenticated user.
- Responsibility or access: Can read and manage customer records in the learning API.

## 4. Functional Requirements

- Requirement: List all customers.
- Expected behavior: Returns seeded and newly created customers.
- Requirement: Get one customer by id.
- Expected behavior: Returns `404 Not Found` when the customer does not exist.
- Requirement: Create a customer.
- Expected behavior: Valid payload returns `201 Created`.
- Requirement: Update a customer.
- Expected behavior: Valid payload replaces editable fields.
- Requirement: Delete a customer.
- Expected behavior: Existing customer deletion returns `204 No Content`.

## 5. API Contract

- Endpoint: `/api/customers`
- Method: `GET`
- Auth: Bearer token
- Request: None
- Response: `200 OK` with an array of customers
- Endpoint: `/api/customers/{id}`
- Method: `GET`
- Auth: Bearer token
- Request: Route `id`
- Response: `200 OK` or `404 Not Found`
- Endpoint: `/api/customers`
- Method: `POST`
- Auth: Bearer token
- Request: `{ fullName, email, phoneNumber }`
- Response: `201 Created`
- Endpoint: `/api/customers/{id}`
- Method: `PUT`
- Auth: Bearer token
- Request: `{ fullName, email, phoneNumber }`
- Response: `204 No Content` or `404 Not Found`
- Endpoint: `/api/customers/{id}`
- Method: `DELETE`
- Auth: Bearer token
- Request: Route `id`
- Response: `204 No Content` or `404 Not Found`

## 6. Data and Rules

- Data model: Customer has `id`, `fullName`, `email`, `phoneNumber`, `createdAtUtc`, and `updatedAtUtc`.
- Validation: Full name is required; email must be a valid email address; phone number is optional.
- Business rules: Emails should be stored trimmed and lowercase.

## 7. Permissions

- Public access: None.
- Authenticated access: All customer endpoints.
- Role-based access: `Admin` and `User`.

## 8. Implementation Approach

- Application layer: Controller delegates state changes to `ICustomerRepository`.
- Infrastructure or persistence: Thread-safe in-memory repository.
- Error handling: Missing records return `404`; validation errors return standard ASP.NET Core validation responses.

## 9. Risks and Edge Cases

- Risk: Duplicate emails can confuse client records.
- Mitigation: The first version documents the limitation; a future feature can add uniqueness checks.
- Risk: Phone formats vary by country.
- Mitigation: Keep phone as optional free text for this learning version.

## 10. Validation Plan

- Automated tests: Authenticated customer list returns seeded data.
- Manual checks: Login, list customers, create a customer, update it, and delete it.
