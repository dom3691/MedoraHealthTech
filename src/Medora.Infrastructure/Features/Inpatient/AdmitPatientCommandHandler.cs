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


namespace Medora.Infrastructure.Features.Inpatient;

public sealed class AdmitPatientCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IAdmitPatientCommandHandler
{
    public async Task<CommandResult> Handle(AdmitPatientCommand command, CancellationToken cancellationToken = default)
    {
        RequireText(command.AdmissionNumber, "Admission number");
        RequireText(command.PatientMrn, "Patient MRN");
        RequireText(command.PatientName, "Patient name");
        RequireText(command.WardName, "Ward name");
        RequireText(command.AttendingProvider, "Attending provider");
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var admission = new Admission
        {
            AdmissionNumber = command.AdmissionNumber.Trim(),
            PatientMrn = command.PatientMrn.Trim(),
            PatientName = command.PatientName.Trim(),
            WardName = command.WardName.Trim(),
            BedNumber = command.BedNumber,
            AttendingProvider = command.AttendingProvider.Trim(),
            Diagnosis = command.Diagnosis,
            AdmittedAt = DateTimeOffset.UtcNow,
            DischargePlannedAt = command.DischargePlannedAt,
            Status = "Admitted"
        };

        StampCreate(admission, tenantId);
        DbContext.Admissions.Add(admission);

        var patient = await DbContext.Patients.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Mrn == admission.PatientMrn, cancellationToken);
        if (patient is not null)
        {
            patient.Status = "Admitted";
            StampUpdate(patient);
        }

        if (!string.IsNullOrWhiteSpace(admission.BedNumber))
        {
            var bed = await DbContext.Beds.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.BedNumber == admission.BedNumber, cancellationToken);
            if (bed is not null)
            {
                bed.PatientMrn = admission.PatientMrn;
                bed.Status = "Occupied";
                StampUpdate(bed);
            }
        }

        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(admission, admission.AdmissionNumber, admission.Status);
    }
}

