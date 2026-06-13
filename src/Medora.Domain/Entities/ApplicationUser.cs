using Medora.Domain.Common;

namespace Medora.Domain.Entities;

public sealed class ApplicationUser : TenantScopedEntity
{
    public required string FullName { get; set; }
    public required string Email { get; set; }
    public string? PasswordHash { get; set; }
    public bool IsActive { get; set; } = true;
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
