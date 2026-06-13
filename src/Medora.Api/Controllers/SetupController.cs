using Medora.Infrastructure.Persistence;
using Medora.Infrastructure.Persistence.Seed;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Medora.Api.Controllers;

[ApiController]
[Route("api/setup")]
public sealed class SetupController : ControllerBase
{
    private readonly MedoraDbContext _dbContext;
    private readonly DatabaseSeeder _databaseSeeder;

    public SetupController(MedoraDbContext dbContext, DatabaseSeeder databaseSeeder)
    {
        _dbContext = dbContext;
        _databaseSeeder = databaseSeeder;
    }

    [HttpGet("catalog")]
    public IActionResult GetCatalog()
    {
        return Ok(new
        {
            Modules = DefaultPermissionCatalog.Modules,
            PermissionCount = DefaultPermissionCatalog.Permissions.Count,
            Roles = DefaultPermissionCatalog.Roles.Select(role => new
            {
                role.Code,
                role.Name,
                role.Description,
                PermissionPatternCount = role.PermissionPatterns.Length
            })
        });
    }

    [HttpPost("seed")]
    public async Task<IActionResult> Seed(CancellationToken cancellationToken)
    {
        await _databaseSeeder.SeedAsync(cancellationToken);

        return Ok(new
        {
            Tenants = await _dbContext.Tenants.CountAsync(cancellationToken),
            Roles = await _dbContext.Roles.CountAsync(cancellationToken),
            Permissions = await _dbContext.Permissions.CountAsync(cancellationToken),
            RolePermissions = await _dbContext.RolePermissions.CountAsync(cancellationToken),
            Users = await _dbContext.Users.CountAsync(cancellationToken),
            OperationalModules = await _dbContext.OperationalModules.CountAsync(cancellationToken),
            Patients = await _dbContext.Patients.CountAsync(cancellationToken),
            StaffMembers = await _dbContext.StaffMembers.CountAsync(cancellationToken)
        });
    }
}


