namespace Medora.Domain.Entities;

public sealed class Role
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Code { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public bool IsSystemRole { get; set; } = true;
    public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
}
