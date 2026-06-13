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


namespace Medora.Infrastructure.Features.Telemedicine;

public sealed class ScheduleTelemedicineSessionCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IScheduleTelemedicineSessionCommandHandler
{
    public async Task<CommandResult> Handle(ScheduleTelemedicineSessionCommand command, CancellationToken cancellationToken = default)
    {
        RequireText(command.SessionNumber, "Session number");
        RequireText(command.PatientName, "Patient name");
        RequireText(command.ProviderName, "Provider name");
        RequireText(command.Channel, "Telemedicine channel");
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var session = new TelemedicineSession
        {
            SessionNumber = command.SessionNumber.Trim(),
            PatientName = command.PatientName.Trim(),
            ProviderName = command.ProviderName.Trim(),
            Channel = command.Channel.Trim(),
            ScheduledAt = command.ScheduledAt,
            DurationMinutes = command.DurationMinutes,
            Status = "Scheduled"
        };
        StampCreate(session, tenantId);
        DbContext.TelemedicineSessions.Add(session);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(session, session.SessionNumber, session.Status);
    }
}

