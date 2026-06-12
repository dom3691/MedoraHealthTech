using Medora.Domain.Common;

namespace Medora.Domain.HrOperations;

public sealed class ShiftAssignment : TenantScopedEntity
{
    public required string ShiftNumber { get; set; }
    public required string StaffName { get; set; }
    public required string Department { get; set; }
    public DateTimeOffset StartsAt { get; set; }
    public DateTimeOffset EndsAt { get; set; }
    public string Status { get; set; } = "Scheduled";
}

public sealed class AttendanceRecord : TenantScopedEntity
{
    public required string StaffName { get; set; }
    public required string Department { get; set; }
    public DateOnly WorkDate { get; set; }
    public TimeOnly? ClockIn { get; set; }
    public TimeOnly? ClockOut { get; set; }
    public string Status { get; set; } = "Present";
}

public sealed class LeaveRequest : TenantScopedEntity
{
    public required string RequestNumber { get; set; }
    public required string StaffName { get; set; }
    public required string LeaveType { get; set; }
    public DateOnly StartsOn { get; set; }
    public DateOnly EndsOn { get; set; }
    public string Status { get; set; } = "Pending";
    public string? ApproverName { get; set; }
}

public sealed class RecruitmentCandidate : TenantScopedEntity
{
    public required string CandidateNumber { get; set; }
    public required string FullName { get; set; }
    public required string Position { get; set; }
    public required string Department { get; set; }
    public string Stage { get; set; } = "Screening";
    public string Status { get; set; } = "Active";
}

public sealed class TrainingRecord : TenantScopedEntity
{
    public required string StaffName { get; set; }
    public required string CourseName { get; set; }
    public DateOnly DueOn { get; set; }
    public DateOnly? CompletedOn { get; set; }
    public string Status { get; set; } = "Due";
}

public sealed class CredentialRecord : TenantScopedEntity
{
    public required string StaffName { get; set; }
    public required string CredentialName { get; set; }
    public DateOnly ExpiresOn { get; set; }
    public string Status { get; set; } = "Active";
}

public sealed class PerformanceReview : TenantScopedEntity
{
    public required string StaffName { get; set; }
    public required string ReviewPeriod { get; set; }
    public decimal Score { get; set; }
    public string Status { get; set; } = "Draft";
    public string? ReviewerName { get; set; }
}
