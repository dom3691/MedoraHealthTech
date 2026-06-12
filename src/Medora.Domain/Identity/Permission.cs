namespace Medora.Domain.Identity;

public sealed class Permission
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Module { get; set; }
    public required string Action { get; set; }
    public required string Code { get; set; }
    public required string Description { get; set; }
}
