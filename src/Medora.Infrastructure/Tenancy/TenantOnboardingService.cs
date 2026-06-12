using System.Security.Cryptography;
using Medora.Application.Common.Exceptions;
using Medora.Application.Modules.Platform.Tenants;
using Medora.Application.Security;
using Medora.Domain.Identity;
using Medora.Infrastructure.Persistence;
using Medora.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.Tenancy;

public sealed class TenantOnboardingService : IPlatformTenantOnboardingService
{
    private readonly MedoraDbContext _dbContext;
    private readonly DatabaseSeeder _databaseSeeder;

    public TenantOnboardingService(MedoraDbContext dbContext, DatabaseSeeder databaseSeeder)
    {
        _dbContext = dbContext;
        _databaseSeeder = databaseSeeder;
    }

    public async Task<TenantOnboardingResult> OnboardAsync(TenantOnboardingRequest request, CancellationToken cancellationToken = default)
    {
        var normalizedCode = NormalizeTenantCode(request.Code);
        var normalizedEmail = request.AdminEmail.Trim().ToLowerInvariant();

        if (await _dbContext.Tenants.AnyAsync(x => x.Code == normalizedCode, cancellationToken))
        {
            throw new ValidationException($"Tenant code '{normalizedCode}' already exists.");
        }

        if (request.LicensedBeds <= 0)
        {
            throw new ValidationException("Licensed beds must be greater than zero.");
        }

        await _databaseSeeder.SeedRolesAndPermissionsAsync(cancellationToken);

        var adminRole = await _dbContext.Roles.SingleAsync(x => x.Code == "SA", cancellationToken);
        var now = DateTimeOffset.UtcNow;
        var temporaryPassword = GenerateTemporaryPassword();

        var tenant = new Tenant
        {
            Name = request.Name.Trim(),
            Code = normalizedCode,
            Location = request.Location?.Trim(),
            LicensedBeds = request.LicensedBeds,
            Plan = request.Plan.Trim(),
            IsActive = true,
            CreatedAt = now
        };

        var adminUser = new ApplicationUser
        {
            TenantId = tenant.Id,
            CreatedAt = now,
            FullName = request.AdminFullName.Trim(),
            Email = normalizedEmail,
            PasswordHash = PasswordHasher.Hash(normalizedCode, normalizedEmail, temporaryPassword),
            IsActive = true
        };

        adminUser.UserRoles.Add(new UserRole { User = adminUser, Role = adminRole });

        await using var transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);

        _dbContext.Tenants.Add(tenant);
        _dbContext.Users.Add(adminUser);
        _dbContext.OperationalModules.AddRange(OperationsSeedData.Modules(tenant.Id, now));

        if (request.IncludeDemoOperationalData)
        {
            AddDemoOperationalData(tenant.Id, now);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        await transaction.CommitAsync(cancellationToken);

        return new TenantOnboardingResult(
            tenant.Id,
            tenant.Code,
            tenant.Name,
            adminUser.Id,
            adminUser.Email,
            temporaryPassword,
            request.IncludeDemoOperationalData);
    }

    private void AddDemoOperationalData(Guid tenantId, DateTimeOffset now)
    {
        _dbContext.Departments.AddRange(OperationsSeedData.Departments(tenantId, now));
        _dbContext.Patients.AddRange(OperationsSeedData.Patients(tenantId, now));
        _dbContext.StaffMembers.AddRange(OperationsSeedData.Staff(tenantId, now));
        _dbContext.Wards.AddRange(OperationsSeedData.Wards(tenantId, now));
        _dbContext.Beds.AddRange(OperationsSeedData.Beds(tenantId, now));
        _dbContext.Appointments.AddRange(OperationsSeedData.Appointments(tenantId, now));
        _dbContext.PatientEncounters.AddRange(OperationsSeedData.PatientEncounters(tenantId, now));
        _dbContext.Admissions.AddRange(OperationsSeedData.Admissions(tenantId, now));
        _dbContext.EmergencyCases.AddRange(OperationsSeedData.EmergencyCases(tenantId, now));
        _dbContext.SurgeryCases.AddRange(OperationsSeedData.SurgeryCases(tenantId, now));
        _dbContext.NursingTasks.AddRange(OperationsSeedData.NursingTasks(tenantId, now));
        _dbContext.DiagnosticOrders.AddRange(OperationsSeedData.DiagnosticOrders(tenantId, now));
        _dbContext.InventoryItems.AddRange(OperationsSeedData.Inventory(tenantId, now));
        _dbContext.Vendors.AddRange(OperationsSeedData.Vendors(tenantId, now));
        _dbContext.PurchaseOrders.AddRange(OperationsSeedData.PurchaseOrders(tenantId, now));
        _dbContext.Notifications.AddRange(OperationsSeedData.Notifications(tenantId, now));
        _dbContext.Invoices.AddRange(OperationsSeedData.Invoices(tenantId, now));
        _dbContext.InsuranceClaims.AddRange(OperationsSeedData.InsuranceClaims(tenantId, now));
        _dbContext.PreAuthorizations.AddRange(OperationsSeedData.PreAuthorizations(tenantId, now));
        _dbContext.TelemedicineSessions.AddRange(OperationsSeedData.TelemedicineSessions(tenantId, now));
        _dbContext.ComplianceControls.AddRange(OperationsSeedData.ComplianceControls(tenantId, now));
        _dbContext.AuditLogs.AddRange(OperationsSeedData.AuditLogs(tenantId, now));
        _dbContext.ReportMetrics.AddRange(OperationsSeedData.ReportMetrics(tenantId, now));
        _dbContext.ShiftAssignments.AddRange(OperationsSeedData.Shifts(tenantId, now));
        _dbContext.AttendanceRecords.AddRange(OperationsSeedData.Attendance(tenantId, now));
        _dbContext.LeaveRequests.AddRange(OperationsSeedData.LeaveRequests(tenantId, now));
        _dbContext.RecruitmentCandidates.AddRange(OperationsSeedData.Candidates(tenantId, now));
        _dbContext.TrainingRecords.AddRange(OperationsSeedData.Training(tenantId, now));
        _dbContext.CredentialRecords.AddRange(OperationsSeedData.Credentials(tenantId, now));
        _dbContext.PerformanceReviews.AddRange(OperationsSeedData.Reviews(tenantId, now));
    }

    private static string NormalizeTenantCode(string code) =>
        code.Trim().ToUpperInvariant();

    private static string GenerateTemporaryPassword()
    {
        Span<byte> bytes = stackalloc byte[12];
        RandomNumberGenerator.Fill(bytes);
        return $"Medora-{Convert.ToBase64String(bytes).Replace("+", "", StringComparison.Ordinal).Replace("/", "", StringComparison.Ordinal).TrimEnd('=')}!";
    }
}
