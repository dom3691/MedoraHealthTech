using Medora.Domain.Common;

namespace Medora.Domain.Entities;

public sealed class StaffMember : TenantScopedEntity
{
    public required string EmployeeNumber { get; set; }
    public required string FullName { get; set; }
    public required string Department { get; set; }
    public required string JobTitle { get; set; }
    public required string EmploymentType { get; set; }
    public required string Status { get; set; }
    public string? WorkEmail { get; set; }
    public DateOnly? StartDate { get; set; }
}
