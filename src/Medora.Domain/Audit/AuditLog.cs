using Medora.Domain.Common;

namespace Medora.Domain.Audit;

public sealed class AuditLog : TenantScopedEntity
{
    public required string Actor { get; set; }
    public required string Action { get; set; }
    public required string TargetType { get; set; }
    public required string TargetId { get; set; }
    public required string Result { get; set; }
    public string? IpAddress { get; set; }
    public string? MetadataJson { get; set; }
}
