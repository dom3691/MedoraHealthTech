using Medora.Domain.Common;

namespace Medora.Domain.HospitalOperations;

public sealed class Patient : TenantScopedEntity
{
    public required string Mrn { get; set; }
    public required string FullName { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? Sex { get; set; }
    public string? Status { get; set; }
}
