namespace Medora.Application.Modules.Operations.Data;

public interface IOperationsDataQueryService
{
    Task<object> GetTenantsAsync(CancellationToken cancellationToken = default);
    Task<object> GetModulesAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetPatientsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetAppointmentsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetEncountersAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetAdmissionsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetEmergencyCasesAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetWardsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetBedsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetSurgeryCasesAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetNursingTasksAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetDiagnosticOrdersAsync(string? tenantCode, string? category, CancellationToken cancellationToken = default);
    Task<object> GetInventoryItemsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetVendorsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetPurchaseOrdersAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetNotificationsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetStaffAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetDepartmentsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetInvoicesAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetInsuranceClaimsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetPreAuthorizationsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetTelemedicineSessionsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetAuditLogsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetComplianceControlsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetReportMetricsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetShiftsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetAttendanceAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetLeaveRequestsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetCandidatesAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetTrainingAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetCredentialsAsync(string? tenantCode, CancellationToken cancellationToken = default);
    Task<object> GetReviewsAsync(string? tenantCode, CancellationToken cancellationToken = default);
}
