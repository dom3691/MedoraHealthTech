namespace Medora.Domain.Identity;

public sealed class Tenant
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    public required string Code { get; set; }
    public string? Location { get; set; }
    public int LicensedBeds { get; set; }
    public string Plan { get; set; } = "Enterprise";
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAt { get; set; }
}
