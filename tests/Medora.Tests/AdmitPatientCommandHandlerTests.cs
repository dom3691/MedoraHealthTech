using Medora.Application.Common.Interfaces;
using Medora.Application.Features.Inpatient.Commands.AdmitPatient;
using Medora.Domain.Entities;
using Medora.Infrastructure.Features.Inpatient;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Medora.Tests;

public sealed class AdmitPatientCommandHandlerTests
{
    [Fact]
    public async Task Handle_CreatesAdmissionAndOccupiesBedForCurrentTenant()
    {
        var tenantId = Guid.NewGuid();
        await using var dbContext = CreateDbContext();
        dbContext.Tenants.Add(new Tenant { Id = tenantId, Name = "St. Mary General Hospital", Code = "SMG", CreatedAt = DateTimeOffset.UtcNow });
        dbContext.Patients.Add(new Patient { TenantId = tenantId, Mrn = "MRN-100", FullName = "Ada Patient", Status = "Outpatient" });
        dbContext.Beds.Add(new Bed { TenantId = tenantId, BedNumber = "CAR-01", WardName = "Cardiac ICU", Status = "Available" });
        await dbContext.SaveChangesAsync();

        var handler = new AdmitPatientCommandHandler(
            dbContext,
            new TestCurrentTenantService(tenantId, "SMG"),
            new TestCurrentUserService("tester@medora.local"));

        var result = await handler.Handle(new AdmitPatientCommand(
            "ADM-100",
            "MRN-100",
            "Ada Patient",
            "Cardiac ICU",
            "CAR-01",
            "Dr. Helen Acharya",
            "Observation",
            null));

        var admission = await dbContext.Admissions.SingleAsync(x => x.Id == result.Id);
        var patient = await dbContext.Patients.SingleAsync(x => x.Mrn == "MRN-100");
        var bed = await dbContext.Beds.SingleAsync(x => x.BedNumber == "CAR-01");

        Assert.Equal("Admitted", admission.Status);
        Assert.Equal("Admitted", patient.Status);
        Assert.Equal("Occupied", bed.Status);
        Assert.Equal("MRN-100", bed.PatientMrn);
        Assert.Equal(tenantId, admission.TenantId);
    }

    [Fact]
    public async Task Handle_RejectsMissingAdmissionNumber()
    {
        var tenantId = Guid.NewGuid();
        await using var dbContext = CreateDbContext();
        dbContext.Tenants.Add(new Tenant { Id = tenantId, Name = "St. Mary General Hospital", Code = "SMG", CreatedAt = DateTimeOffset.UtcNow });
        await dbContext.SaveChangesAsync();

        var handler = new AdmitPatientCommandHandler(
            dbContext,
            new TestCurrentTenantService(tenantId, "SMG"),
            new TestCurrentUserService("tester@medora.local"));

        await Assert.ThrowsAsync<Medora.Application.Common.Exceptions.ValidationException>(() =>
            handler.Handle(new AdmitPatientCommand(
                "",
                "MRN-100",
                "Ada Patient",
                "Cardiac ICU",
                "CAR-01",
                "Dr. Helen Acharya",
                null,
                null)));
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

    private sealed class TestCurrentUserService(string email) : ICurrentUserService
    {
        public Guid? UserId => null;

        public string? Email { get; } = email;

        public string? FullName => "Test User";

        public IReadOnlyCollection<string> Roles => Array.Empty<string>();
    }
}
