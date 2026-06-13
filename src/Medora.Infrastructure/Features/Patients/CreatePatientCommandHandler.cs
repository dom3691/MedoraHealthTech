using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;
using Medora.Application.Features.Patients.Commands.CreatePatient;
using Medora.Domain.Entities;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.Features.Patients;

public sealed class CreatePatientCommandHandler(
    MedoraDbContext dbContext,
    ICurrentTenantService currentTenant,
    ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), ICreatePatientCommandHandler
{
    public async Task<CommandResult> Handle(CreatePatientCommand command, CancellationToken cancellationToken = default)
    {
        Require(!string.IsNullOrWhiteSpace(command.Mrn), "MRN is required.");
        Require(!string.IsNullOrWhiteSpace(command.FullName), "Patient name is required.");

        var tenantId = await GetTenantIdAsync(cancellationToken);
        var exists = await DbContext.Patients.AnyAsync(x => x.TenantId == tenantId && x.Mrn == command.Mrn, cancellationToken);
        Require(!exists, $"Patient with MRN '{command.Mrn}' already exists.");

        var patient = new Patient
        {
            Mrn = command.Mrn.Trim(),
            FullName = command.FullName.Trim(),
            DateOfBirth = command.DateOfBirth,
            Sex = command.Sex,
            Status = string.IsNullOrWhiteSpace(command.Status) ? "Outpatient" : command.Status.Trim()
        };

        StampCreate(patient, tenantId);
        DbContext.Patients.Add(patient);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result(patient, patient.Mrn, patient.Status ?? "Created");
    }
}

