# Medora Backend Clean Architecture

## Project Boundaries

The backend follows a Clean Architecture dependency direction:

- `Medora.Domain`: enterprise entities and tenant-scoped domain model. No framework dependencies.
- `Medora.Application`: use-case contracts, module request/response models, application exceptions, and abstractions. Depends only on Domain.
- `Medora.Infrastructure`: EF Core, SQL Server, seeding, query implementations, and application service implementations.
- `Medora.Api`: HTTP controllers, authentication/authorization policy setup, JWT token issuance, Swagger, CORS, and health checks.

Dependency flow:

`Api -> Application <- Infrastructure -> Domain`

`Application -> Domain`

`Domain` depends on nothing.

## Module Structure

Application contracts are grouped by business capability:

- `Modules/Identity/Auth`
- `Modules/Platform/Tenants`
- `Modules/TenantAdministration/Users`
- Future module groups should follow the same structure:
  - `Modules/HospitalOperations/Patients`
  - `Modules/HospitalOperations/Appointments`
  - `Modules/HospitalOperations/PatientFlow`
  - `Modules/HospitalOperations/Inpatient`
  - `Modules/HospitalOperations/Emergency`
  - `Modules/HospitalOperations/Diagnostics`
  - `Modules/HospitalOperations/Pharmacy`
  - `Modules/HrOperations/Staff`
  - `Modules/HrOperations/Attendance`
  - `Modules/HrOperations/Leave`
  - `Modules/Finance/Billing`
  - `Modules/Finance/Insurance`
  - `Modules/AuditCompliance/Audit`

Infrastructure implementations should mirror the module names where practical. For example:

- `Infrastructure/Identity/AuthService`
- `Infrastructure/Tenancy/TenantOnboardingService`
- `Infrastructure/TenantAdministration/TenantUserOnboardingService`

## Controller Rules

Controllers should:

- Accept HTTP requests.
- Read route/query/body parameters.
- Resolve user/tenant claims.
- Call one Application service.
- Translate application exceptions into HTTP responses.

Controllers should not:

- Query `MedoraDbContext` directly for business workflows.
- Hash passwords.
- Generate temporary passwords.
- Decide role assignment rules.
- Contain tenant onboarding or employee onboarding logic.

## Security Rules

- `MSA` is the Medora platform Super Admin role.
- `MSA` can onboard client tenants.
- Tenant `SA` is the Tenant Super Admin role.
- Tenant `SA` cannot create tenants.
- Tenant `SA` and `HR` can onboard employees/users only within their own tenant.
- The backend enforces these rules using JWT role claims and authorization policies.

## Next Refactor Targets

The read-heavy `OperationsDataController` is still a screen/prototype data façade. As modules become production-grade, split those endpoints into module query services and DTOs under the module folders above. Do this module by module to avoid breaking the current UI contract.
