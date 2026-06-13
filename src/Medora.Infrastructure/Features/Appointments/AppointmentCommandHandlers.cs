using Medora.Application.Common.Exceptions;
using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;
using Medora.Application.Features.Appointments.Commands.CreateAppointment;
using Medora.Application.Features.Appointments.Commands.UpdateAppointmentStatus;
using Medora.Domain.Entities;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.Features.Appointments;

public sealed class CreateAppointmentCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), ICreateAppointmentCommandHandler
{
    public async Task<CommandResult> Handle(CreateAppointmentCommand command, CancellationToken cancellationToken = default)
    {
        Require(!string.IsNullOrWhiteSpace(command.AppointmentNumber), "Appointment number is required.");
        Require(!string.IsNullOrWhiteSpace(command.PatientMrn), "Patient MRN is required.");
        Require(!string.IsNullOrWhiteSpace(command.PatientName), "Patient name is required.");
        RequireText(command.ProviderName, "Provider name");
        RequireText(command.Department, "Department");
        RequireText(command.VisitType, "Visit type");

        var tenantId = await GetTenantIdAsync(cancellationToken);
        var appointment = new Appointment
        {
            AppointmentNumber = command.AppointmentNumber.Trim(),
            PatientMrn = command.PatientMrn.Trim(),
            PatientName = command.PatientName.Trim(),
            ProviderName = command.ProviderName.Trim(),
            Department = command.Department.Trim(),
            VisitType = command.VisitType.Trim(),
            ScheduledAt = command.ScheduledAt,
            Room = command.Room,
            Status = "Confirmed"
        };

        StampCreate(appointment, tenantId);
        DbContext.Appointments.Add(appointment);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result(appointment, appointment.AppointmentNumber, appointment.Status);
    }
}

public sealed class UpdateAppointmentStatusCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IUpdateAppointmentStatusCommandHandler
{
    public async Task<CommandResult> Handle(UpdateAppointmentStatusCommand command, CancellationToken cancellationToken = default)
    {
        RequireText(command.Status, "Appointment status");

        var tenantId = await GetTenantIdAsync(cancellationToken);
        var appointment = await DbContext.Appointments.FirstOrDefaultAsync(x => x.Id == command.AppointmentId && x.TenantId == tenantId, cancellationToken)
            ?? throw new ValidationException("Appointment was not found.");

        appointment.Status = command.Status.Trim();
        StampUpdate(appointment);
        await DbContext.SaveChangesAsync(cancellationToken);

        return Result(appointment, appointment.AppointmentNumber, appointment.Status);
    }
}

