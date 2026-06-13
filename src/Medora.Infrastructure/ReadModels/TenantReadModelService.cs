using Medora.Application.Common.Interfaces;
using Medora.Domain.Common;
using Medora.Domain.Entities;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.ReadModels;

public sealed class TenantReadModelService(MedoraDbContext dbContext) : ITenantReadModelService
{
    public async Task<object> GetTenantsAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Tenants
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Code)
            .Select(x => new
            {
                x.Id,
                x.Code,
                x.Name,
                x.Location,
                x.LicensedBeds,
                x.Plan
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<object> GetModulesAsync(string? tenantCode, CancellationToken cancellationToken = default)
    {
        var tenant = await ResolveTenantAsync(tenantCode, cancellationToken);
        return await dbContext.OperationalModules
            .AsNoTracking()
            .Where(x => x.TenantId == tenant.Id && !x.IsDeleted)
            .OrderBy(x => x.SortOrder)
            .ToListAsync(cancellationToken);
    }

    public async Task<object> GetPatientsAsync(string? tenantCode, CancellationToken cancellationToken = default)
    {
        var tenant = await ResolveTenantAsync(tenantCode, cancellationToken);
        return await dbContext.Patients
            .AsNoTracking()
            .Where(x => x.TenantId == tenant.Id && !x.IsDeleted)
            .OrderBy(x => x.FullName)
            .ToListAsync(cancellationToken);
    }

    public async Task<object> GetAppointmentsAsync(string? tenantCode, CancellationToken cancellationToken = default)
    {
        var tenant = await ResolveTenantAsync(tenantCode, cancellationToken);
        return await dbContext.Appointments
            .AsNoTracking()
            .Where(x => x.TenantId == tenant.Id && !x.IsDeleted)
            .OrderBy(x => x.ScheduledAt)
            .ToListAsync(cancellationToken);
    }

    public Task<object> GetEncountersAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.PatientEncounters.OrderBy(x => x.ArrivedAt), cancellationToken);

    public Task<object> GetAdmissionsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.Admissions.OrderByDescending(x => x.AdmittedAt), cancellationToken);

    public Task<object> GetEmergencyCasesAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.EmergencyCases.OrderBy(x => x.ArrivedAt), cancellationToken);

    public Task<object> GetWardsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.Wards.OrderBy(x => x.Name), cancellationToken);

    public Task<object> GetBedsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.Beds.OrderBy(x => x.BedNumber), cancellationToken);

    public Task<object> GetSurgeryCasesAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.SurgeryCases.OrderBy(x => x.ScheduledAt), cancellationToken);

    public Task<object> GetNursingTasksAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.NursingTasks.OrderBy(x => x.DueAt), cancellationToken);

    public async Task<object> GetDiagnosticOrdersAsync(string? tenantCode, string? category, CancellationToken cancellationToken = default)
    {
        var tenant = await ResolveTenantAsync(tenantCode, cancellationToken);
        var query = dbContext.DiagnosticOrders
            .AsNoTracking()
            .Where(x => x.TenantId == tenant.Id && !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(category))
        {
            query = query.Where(x => x.Category == category);
        }

        return await query.OrderBy(x => x.OrderedAt).ToListAsync(cancellationToken);
    }

    public Task<object> GetInventoryItemsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.InventoryItems.OrderBy(x => x.Name), cancellationToken);

    public Task<object> GetVendorsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.Vendors.OrderBy(x => x.Name), cancellationToken);

    public Task<object> GetPurchaseOrdersAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.PurchaseOrders.OrderByDescending(x => x.CreatedAt), cancellationToken);

    public Task<object> GetNotificationsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.Notifications.OrderByDescending(x => x.PublishedAt), cancellationToken);

    public Task<object> GetStaffAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.StaffMembers.OrderBy(x => x.FullName), cancellationToken);

    public Task<object> GetDepartmentsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.Departments.OrderBy(x => x.Name), cancellationToken);

    public Task<object> GetInvoicesAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.Invoices.OrderByDescending(x => x.IssuedOn), cancellationToken);

    public Task<object> GetInsuranceClaimsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.InsuranceClaims.OrderByDescending(x => x.SubmittedOn), cancellationToken);

    public Task<object> GetPreAuthorizationsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.PreAuthorizations.OrderBy(x => x.PatientName), cancellationToken);

    public Task<object> GetTelemedicineSessionsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.TelemedicineSessions.OrderBy(x => x.ScheduledAt), cancellationToken);

    public Task<object> GetAuditLogsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.AuditLogs.OrderByDescending(x => x.CreatedAt), cancellationToken);

    public Task<object> GetComplianceControlsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.ComplianceControls.OrderBy(x => x.Name), cancellationToken);

    public Task<object> GetReportMetricsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.ReportMetrics.OrderBy(x => x.Area).ThenBy(x => x.Name), cancellationToken);

    public Task<object> GetShiftsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.ShiftAssignments.OrderBy(x => x.StartsAt), cancellationToken);

    public Task<object> GetAttendanceAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.AttendanceRecords.OrderByDescending(x => x.WorkDate), cancellationToken);

    public Task<object> GetLeaveRequestsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.LeaveRequests.OrderByDescending(x => x.CreatedAt), cancellationToken);

    public Task<object> GetCandidatesAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.RecruitmentCandidates.OrderBy(x => x.FullName), cancellationToken);

    public Task<object> GetTrainingAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.TrainingRecords.OrderBy(x => x.DueOn), cancellationToken);

    public Task<object> GetCredentialsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.CredentialRecords.OrderBy(x => x.ExpiresOn), cancellationToken);

    public Task<object> GetReviewsAsync(string? tenantCode, CancellationToken cancellationToken = default) =>
        TenantSetAsync(tenantCode, dbContext.PerformanceReviews.OrderByDescending(x => x.ReviewPeriod), cancellationToken);

    private async Task<object> TenantSetAsync<TEntity>(
        string? tenantCode,
        IQueryable<TEntity> query,
        CancellationToken cancellationToken)
        where TEntity : TenantScopedEntity
    {
        var tenant = await ResolveTenantAsync(tenantCode, cancellationToken);
        return await query
            .AsNoTracking()
            .Where(x => x.TenantId == tenant.Id && !x.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    private async Task<Tenant> ResolveTenantAsync(string? tenantCode, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(tenantCode))
        {
            var normalized = tenantCode.Trim();
            var tenant = await dbContext.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Code == normalized && x.IsActive, cancellationToken);

            if (tenant is not null)
            {
                return tenant;
            }
        }

        return await dbContext.Tenants
            .AsNoTracking()
            .OrderBy(x => x.Code)
            .FirstAsync(x => x.IsActive, cancellationToken);
    }
}


