using Medora.Application.Common.Exceptions;
using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;
using Medora.Application.Features.Emergency.Commands.RegisterEmergencyArrival;
using Medora.Application.Features.Emergency.Commands.UpdateEmergencyStatus;
using Medora.Application.Features.Inpatient.Commands.AdmitPatient;
using Medora.Application.Features.Inpatient.Commands.DischargePatient;
using Medora.Application.Features.LabDiagnostics.Commands.CreateLabOrder;
using Medora.Application.Features.LabDiagnostics.Commands.UpdateLabOrderStatus;
using Medora.Application.Features.Nursing.Commands.CreateNursingTask;
using Medora.Application.Features.Nursing.Commands.UpdateNursingTaskStatus;
using Medora.Application.Features.Outpatient.Commands.BookOutpatientVisit;
using Medora.Application.Features.PatientFlow.Commands.MovePatientFlowStage;
using Medora.Application.Features.PatientFlow.Commands.RegisterWalkIn;
using Medora.Application.Features.PharmacyInventory.Commands.AddInventoryItem;
using Medora.Application.Features.PharmacyInventory.Commands.ReorderStock;
using Medora.Application.Features.ProcurementVendors.Commands.CreatePurchaseOrder;
using Medora.Application.Features.RadiologyImaging.Commands.CreateRadiologyStudy;
using Medora.Application.Features.RadiologyImaging.Commands.UpdateRadiologyStudyStatus;
using Medora.Application.Features.Telemedicine.Commands.ScheduleTelemedicineSession;
using Medora.Application.Features.TheatreAndSurgery.Commands.ScheduleSurgery;
using Medora.Application.Features.TheatreAndSurgery.Commands.UpdateSurgeryStatus;
using Medora.Application.Features.WardsAndBeds.Commands.AllocateBed;
using Medora.Application.Features.WardsAndBeds.Commands.UpdateBedStatus;
using Medora.Domain.Entities;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;


namespace Medora.Infrastructure.Features.Outpatient;

public sealed class BookOutpatientVisitCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IBookOutpatientVisitCommandHandler
{
    public async Task<CommandResult> Handle(BookOutpatientVisitCommand command, CancellationToken cancellationToken = default)
    {
        RequireText(command.PatientMrn, "Patient MRN");
        RequireText(command.PatientName, "Patient name");
        RequireText(command.ProviderName, "Provider name");
        RequireText(command.Department, "Department");
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

