using Medora.Infrastructure.Persistence;
using Medora.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Medora.Tests;

public sealed class DatabaseSeederTests
{
    [Fact]
    public async Task SeedAsync_CreatesRolesPermissionsAndTenant()
    {
        await using var dbContext = CreateDbContext();
        var seeder = new DatabaseSeeder(dbContext);

        await seeder.SeedAsync();

        Assert.Equal(DefaultPermissionCatalog.Roles.Count, await dbContext.Roles.CountAsync());
        Assert.Equal(DefaultPermissionCatalog.Permissions.Count, await dbContext.Permissions.CountAsync());
        Assert.Equal(4, await dbContext.Tenants.CountAsync());
        Assert.Equal(4, await dbContext.Users.CountAsync());
        Assert.True(await dbContext.Tenants.AnyAsync(x => x.Code == "MEDORA"));
        Assert.True(await dbContext.Users.AnyAsync(x => x.Email == "superadmin@medora.local"));
        Assert.Equal(104, await dbContext.OperationalModules.CountAsync());
        Assert.True(await dbContext.Patients.AnyAsync());
        Assert.True(await dbContext.StaffMembers.AnyAsync());
        Assert.True(await dbContext.RolePermissions.AnyAsync());
    }

    [Fact]
    public async Task SeedAsync_IsIdempotent()
    {
        await using var dbContext = CreateDbContext();
        var seeder = new DatabaseSeeder(dbContext);

        await seeder.SeedAsync();
        await seeder.SeedAsync();

        Assert.Equal(DefaultPermissionCatalog.Roles.Count, await dbContext.Roles.CountAsync());
        Assert.Equal(DefaultPermissionCatalog.Permissions.Count, await dbContext.Permissions.CountAsync());
        Assert.Equal(4, await dbContext.Tenants.CountAsync());
        Assert.Equal(4, await dbContext.Users.CountAsync());
        Assert.Equal(104, await dbContext.OperationalModules.CountAsync());
    }

    private static MedoraDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<MedoraDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new MedoraDbContext(options);
    }
}
