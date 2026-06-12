using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;
using Medora.Application.Features.Patients.Queries.GetPatients;
using Medora.Domain.HospitalOperations;
using Medora.Domain.Identity;
using Medora.Infrastructure.Features.Patients;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Medora.Tests;

public sealed class PatientsQueryHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsOnlyPatientsForCurrentTenant()
    {
        var tenantAId = Guid.NewGuid();
        var tenantBId = Guid.NewGuid();

        await using var dbContext = CreateDbContext();
        dbContext.Tenants.AddRange(
            new Tenant { Id = tenantAId, Name = "St. Mary General Hospital", Code = "SMG", CreatedAt = DateTimeOffset.UtcNow },
            new Tenant { Id = tenantBId, Name = "Riverside Medical Center", Code = "RVS", CreatedAt = DateTimeOffset.UtcNow });
        dbContext.Patients.AddRange(
            new Patient { TenantId = tenantAId, Mrn = "MRN-001", FullName = "Ada Tenant", Status = "Admitted" },
            new Patient { TenantId = tenantBId, Mrn = "MRN-002", FullName = "Ben Other", Status = "Admitted" });
        await dbContext.SaveChangesAsync();

        var handler = new GetPatientsQueryHandler(dbContext, new TestCurrentTenantService(tenantAId, "SMG"));
        var result = await handler.Handle(new GetPatientsQuery(new QueryParameters()));

        Assert.Equal(1, result.TotalCount);
        Assert.Equal("MRN-001", result.Items.Single().Mrn);
    }

    private static MedoraDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<MedoraDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new MedoraDbContext(options);
    }

    private sealed class TestCurrentTenantService(Guid? tenantId, string? tenantCode) : ICurrentTenantService
    {
        public Guid? TenantId { get; } = tenantId;

        public string? TenantCode { get; } = tenantCode;
    }
}
