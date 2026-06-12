using Medora.Domain.Common;

namespace Medora.Domain.HospitalOperations;

public sealed class OperationalModule : TenantScopedEntity
{
    public required string Key { get; set; }
    public required string Label { get; set; }
    public required string Route { get; set; }
    public required string Group { get; set; }
    public required string Icon { get; set; }
    public required string Title { get; set; }
    public required string Subtitle { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

public sealed class Department : TenantScopedEntity
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Type { get; set; }
    public string? HeadName { get; set; }
    public int StaffCount { get; set; }
    public int BedCount { get; set; }
    public decimal OccupancyRate { get; set; }
    public string Status { get; set; } = "Active";
}

public sealed class Ward : TenantScopedEntity
{
    public required string Code { get; set; }
    public required string Name { get; set; }
    public string? Department { get; set; }
    public int TotalBeds { get; set; }
    public int OccupiedBeds { get; set; }
    public string Status { get; set; } = "Open";
}

public sealed class Bed : TenantScopedEntity
{
    public required string BedNumber { get; set; }
    public required string WardName { get; set; }
    public string Status { get; set; } = "Available";
    public string? PatientMrn { get; set; }
}

public sealed class Appointment : TenantScopedEntity
{
    public required string AppointmentNumber { get; set; }
    public required string PatientMrn { get; set; }
    public required string PatientName { get; set; }
    public required string ProviderName { get; set; }
    public required string Department { get; set; }
    public required string VisitType { get; set; }
    public DateTimeOffset ScheduledAt { get; set; }
    public string? Room { get; set; }
    public string Status { get; set; } = "Confirmed";
}

public sealed class PatientEncounter : TenantScopedEntity
{
    public required string EncounterNumber { get; set; }
    public string? PatientMrn { get; set; }
    public required string PatientName { get; set; }
    public required string EntryPoint { get; set; }
    public string Stage { get; set; } = "Registered";
    public string? Complaint { get; set; }
    public string? PayerType { get; set; }
    public string? Insurer { get; set; }
    public string? AssignedProvider { get; set; }
    public DateTimeOffset ArrivedAt { get; set; }
    public int WaitMinutes { get; set; }
    public string Status { get; set; } = "Active";
}

public sealed class Admission : TenantScopedEntity
{
    public required string AdmissionNumber { get; set; }
    public required string PatientMrn { get; set; }
    public required string PatientName { get; set; }
    public required string WardName { get; set; }
    public string? BedNumber { get; set; }
    public required string AttendingProvider { get; set; }
    public DateTimeOffset AdmittedAt { get; set; }
    public DateTimeOffset? DischargePlannedAt { get; set; }
    public string Status { get; set; } = "Admitted";
    public string? Diagnosis { get; set; }
}

public sealed class EmergencyCase : TenantScopedEntity
{
    public required string CaseNumber { get; set; }
    public required string PatientName { get; set; }
    public required string Acuity { get; set; }
    public required string Complaint { get; set; }
    public DateTimeOffset ArrivedAt { get; set; }
    public int WaitMinutes { get; set; }
    public string Status { get; set; } = "Awaiting triage";
}

public sealed class SurgeryCase : TenantScopedEntity
{
    public required string CaseNumber { get; set; }
    public required string PatientName { get; set; }
    public required string Procedure { get; set; }
    public required string SurgeonName { get; set; }
    public required string Theatre { get; set; }
    public DateTimeOffset ScheduledAt { get; set; }
    public string Status { get; set; } = "Scheduled";
}

public sealed class NursingTask : TenantScopedEntity
{
    public required string TaskNumber { get; set; }
    public required string PatientName { get; set; }
    public required string WardName { get; set; }
    public required string TaskType { get; set; }
    public DateTimeOffset DueAt { get; set; }
    public required string NurseName { get; set; }
    public string Status { get; set; } = "Due";
}

public sealed class DiagnosticOrder : TenantScopedEntity
{
    public required string OrderNumber { get; set; }
    public required string Category { get; set; }
    public required string PatientName { get; set; }
    public required string TestName { get; set; }
    public string? BodyPart { get; set; }
    public string Priority { get; set; } = "Routine";
    public string Status { get; set; } = "Pending";
    public DateTimeOffset OrderedAt { get; set; }
    public string? AssignedTo { get; set; }
    public string? Finding { get; set; }
}

public sealed class InventoryItem : TenantScopedEntity
{
    public required string Sku { get; set; }
    public required string Name { get; set; }
    public required string Category { get; set; }
    public int QuantityOnHand { get; set; }
    public int ReorderLevel { get; set; }
    public required string Unit { get; set; }
    public DateOnly? ExpiryDate { get; set; }
    public string Status { get; set; } = "In stock";
}

public sealed class Vendor : TenantScopedEntity
{
    public required string VendorCode { get; set; }
    public required string Name { get; set; }
    public string? Category { get; set; }
    public int OpenOrders { get; set; }
    public decimal OnTimeRate { get; set; }
    public string Status { get; set; } = "Active";
}

public sealed class PurchaseOrder : TenantScopedEntity
{
    public required string PurchaseOrderNumber { get; set; }
    public required string ItemName { get; set; }
    public required string VendorName { get; set; }
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }
    public DateOnly? ExpectedDate { get; set; }
    public string Status { get; set; } = "Draft";
}

public sealed class NotificationMessage : TenantScopedEntity
{
    public required string Category { get; set; }
    public required string Title { get; set; }
    public required string Body { get; set; }
    public string Severity { get; set; } = "Info";
    public string Status { get; set; } = "Unread";
    public DateTimeOffset PublishedAt { get; set; }
}

public sealed class Invoice : TenantScopedEntity
{
    public required string InvoiceNumber { get; set; }
    public required string PatientName { get; set; }
    public required string PayerType { get; set; }
    public decimal Amount { get; set; }
    public DateOnly IssuedOn { get; set; }
    public DateOnly DueOn { get; set; }
    public string Status { get; set; } = "Pending";
}

public sealed class InsuranceClaim : TenantScopedEntity
{
    public required string ClaimNumber { get; set; }
    public required string PatientName { get; set; }
    public required string InsurerName { get; set; }
    public decimal Amount { get; set; }
    public DateOnly SubmittedOn { get; set; }
    public string Status { get; set; } = "Submitted";
}

public sealed class PreAuthorization : TenantScopedEntity
{
    public required string PatientName { get; set; }
    public required string ServiceName { get; set; }
    public required string InsurerName { get; set; }
    public string Status { get; set; } = "Pending";
}

public sealed class TelemedicineSession : TenantScopedEntity
{
    public required string SessionNumber { get; set; }
    public required string PatientName { get; set; }
    public required string ProviderName { get; set; }
    public required string Channel { get; set; }
    public DateTimeOffset ScheduledAt { get; set; }
    public int DurationMinutes { get; set; }
    public string Status { get; set; } = "Scheduled";
}

public sealed class ComplianceControl : TenantScopedEntity
{
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string Status { get; set; } = "Compliant";
    public DateOnly? DueOn { get; set; }
}

public sealed class ReportMetric : TenantScopedEntity
{
    public required string Area { get; set; }
    public required string Name { get; set; }
    public required string Value { get; set; }
    public string? Trend { get; set; }
    public DateOnly PeriodStart { get; set; }
    public DateOnly PeriodEnd { get; set; }
}
