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
using Medora.Domain.HospitalOperations;
using Medora.Infrastructure.Features.Common;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.Features.HospitalOperations;

public sealed class RegisterWalkInCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IRegisterWalkInCommandHandler
{
    public async Task<CommandResult> Handle(RegisterWalkInCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var encounter = new PatientEncounter
        {
            EncounterNumber = command.EncounterNumber.Trim(),
            PatientMrn = command.PatientMrn,
            PatientName = command.PatientName.Trim(),
            EntryPoint = "Walk-in",
            Stage = "Attendant's floor",
            Complaint = command.Complaint,
            PayerType = command.PayerType,
            Insurer = command.Insurer,
            ArrivedAt = DateTimeOffset.UtcNow,
            Status = "Active"
        };

        StampCreate(encounter, tenantId);
        DbContext.PatientEncounters.Add(encounter);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(encounter, encounter.EncounterNumber, encounter.Stage);
    }
}

public sealed class MovePatientFlowStageCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IMovePatientFlowStageCommandHandler
{
    public async Task<CommandResult> Handle(MovePatientFlowStageCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var encounter = await DbContext.PatientEncounters.FirstOrDefaultAsync(x => x.Id == command.EncounterId && x.TenantId == tenantId, cancellationToken)
            ?? throw new ValidationException("Patient flow encounter was not found.");
        encounter.Stage = command.Stage.Trim();
        encounter.Status = command.Status.Trim();
        StampUpdate(encounter);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(encounter, encounter.EncounterNumber, encounter.Stage);
    }
}

public sealed class BookOutpatientVisitCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IBookOutpatientVisitCommandHandler
{
    public async Task<CommandResult> Handle(BookOutpatientVisitCommand command, CancellationToken cancellationToken = default)
    {
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

public sealed class AdmitPatientCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IAdmitPatientCommandHandler
{
    public async Task<CommandResult> Handle(AdmitPatientCommand command, CancellationToken cancellationToken = default)
    {
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

public sealed class DischargePatientCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IDischargePatientCommandHandler
{
    public async Task<CommandResult> Handle(DischargePatientCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var admission = await DbContext.Admissions.FirstOrDefaultAsync(x => x.Id == command.AdmissionId && x.TenantId == tenantId, cancellationToken)
            ?? throw new ValidationException("Admission was not found.");
        admission.Status = "Discharged";
        StampUpdate(admission);

        var patient = await DbContext.Patients.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.Mrn == admission.PatientMrn, cancellationToken);
        if (patient is not null)
        {
            patient.Status = "Discharged";
            StampUpdate(patient);
        }

        if (!string.IsNullOrWhiteSpace(admission.BedNumber))
        {
            var bed = await DbContext.Beds.FirstOrDefaultAsync(x => x.TenantId == tenantId && x.BedNumber == admission.BedNumber, cancellationToken);
            if (bed is not null)
            {
                bed.PatientMrn = null;
                bed.Status = "Cleaning";
                StampUpdate(bed);
            }
        }

        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(admission, admission.AdmissionNumber, admission.Status);
    }
}

public sealed class RegisterEmergencyArrivalCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IRegisterEmergencyArrivalCommandHandler
{
    public async Task<CommandResult> Handle(RegisterEmergencyArrivalCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var emergencyCase = new EmergencyCase
        {
            CaseNumber = command.CaseNumber.Trim(),
            PatientName = command.PatientName.Trim(),
            Acuity = command.Acuity.Trim(),
            Complaint = command.Complaint.Trim(),
            ArrivedAt = DateTimeOffset.UtcNow,
            Status = "Awaiting triage"
        };
        StampCreate(emergencyCase, tenantId);
        DbContext.EmergencyCases.Add(emergencyCase);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(emergencyCase, emergencyCase.CaseNumber, emergencyCase.Status);
    }
}

public sealed class UpdateEmergencyStatusCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IUpdateEmergencyStatusCommandHandler
{
    public async Task<CommandResult> Handle(UpdateEmergencyStatusCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var emergencyCase = await DbContext.EmergencyCases.FirstOrDefaultAsync(x => x.Id == command.CaseId && x.TenantId == tenantId, cancellationToken)
            ?? throw new ValidationException("Emergency case was not found.");
        emergencyCase.Status = command.Status.Trim();
        StampUpdate(emergencyCase);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(emergencyCase, emergencyCase.CaseNumber, emergencyCase.Status);
    }
}

public sealed class AllocateBedCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IAllocateBedCommandHandler
{
    public async Task<CommandResult> Handle(AllocateBedCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var bed = await DbContext.Beds.FirstOrDefaultAsync(x => x.Id == command.BedId && x.TenantId == tenantId, cancellationToken)
            ?? throw new ValidationException("Bed was not found.");
        bed.PatientMrn = command.PatientMrn.Trim();
        bed.Status = "Occupied";
        StampUpdate(bed);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(bed, bed.BedNumber, bed.Status);
    }
}

public sealed class UpdateBedStatusCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IUpdateBedStatusCommandHandler
{
    public async Task<CommandResult> Handle(UpdateBedStatusCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var bed = await DbContext.Beds.FirstOrDefaultAsync(x => x.Id == command.BedId && x.TenantId == tenantId, cancellationToken)
            ?? throw new ValidationException("Bed was not found.");
        bed.Status = command.Status.Trim();
        if (bed.Status is "Available" or "Cleaning")
        {
            bed.PatientMrn = null;
        }
        StampUpdate(bed);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(bed, bed.BedNumber, bed.Status);
    }
}

public sealed class ScheduleSurgeryCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IScheduleSurgeryCommandHandler
{
    public async Task<CommandResult> Handle(ScheduleSurgeryCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var surgery = new SurgeryCase
        {
            CaseNumber = command.CaseNumber.Trim(),
            PatientName = command.PatientName.Trim(),
            Procedure = command.Procedure.Trim(),
            SurgeonName = command.SurgeonName.Trim(),
            Theatre = command.Theatre.Trim(),
            ScheduledAt = command.ScheduledAt,
            Status = "Scheduled"
        };
        StampCreate(surgery, tenantId);
        DbContext.SurgeryCases.Add(surgery);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(surgery, surgery.CaseNumber, surgery.Status);
    }
}

public sealed class UpdateSurgeryStatusCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IUpdateSurgeryStatusCommandHandler
{
    public async Task<CommandResult> Handle(UpdateSurgeryStatusCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var surgery = await DbContext.SurgeryCases.FirstOrDefaultAsync(x => x.Id == command.CaseId && x.TenantId == tenantId, cancellationToken)
            ?? throw new ValidationException("Surgery case was not found.");
        surgery.Status = command.Status.Trim();
        StampUpdate(surgery);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(surgery, surgery.CaseNumber, surgery.Status);
    }
}

public sealed class CreateNursingTaskCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), ICreateNursingTaskCommandHandler
{
    public async Task<CommandResult> Handle(CreateNursingTaskCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var task = new NursingTask
        {
            TaskNumber = command.TaskNumber.Trim(),
            PatientName = command.PatientName.Trim(),
            WardName = command.WardName.Trim(),
            TaskType = command.TaskType.Trim(),
            DueAt = command.DueAt,
            NurseName = command.NurseName.Trim(),
            Status = "Due"
        };
        StampCreate(task, tenantId);
        DbContext.NursingTasks.Add(task);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(task, task.TaskNumber, task.Status);
    }
}

public sealed class UpdateNursingTaskStatusCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IUpdateNursingTaskStatusCommandHandler
{
    public async Task<CommandResult> Handle(UpdateNursingTaskStatusCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var task = await DbContext.NursingTasks.FirstOrDefaultAsync(x => x.Id == command.TaskId && x.TenantId == tenantId, cancellationToken)
            ?? throw new ValidationException("Nursing task was not found.");
        task.Status = command.Status.Trim();
        StampUpdate(task);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(task, task.TaskNumber, task.Status);
    }
}

public sealed class CreateLabOrderCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), ICreateLabOrderCommandHandler
{
    public async Task<CommandResult> Handle(CreateLabOrderCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var order = new DiagnosticOrder
        {
            OrderNumber = command.OrderNumber.Trim(),
            Category = "Lab",
            PatientName = command.PatientName.Trim(),
            TestName = command.TestName.Trim(),
            Priority = command.Priority.Trim(),
            Status = "Pending",
            OrderedAt = DateTimeOffset.UtcNow,
            AssignedTo = command.AssignedTo
        };
        StampCreate(order, tenantId);
        DbContext.DiagnosticOrders.Add(order);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(order, order.OrderNumber, order.Status);
    }
}

public sealed class UpdateLabOrderStatusCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IUpdateLabOrderStatusCommandHandler
{
    public async Task<CommandResult> Handle(UpdateLabOrderStatusCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var order = await DbContext.DiagnosticOrders.FirstOrDefaultAsync(x => x.Id == command.OrderId && x.TenantId == tenantId && x.Category == "Lab", cancellationToken)
            ?? throw new ValidationException("Lab order was not found.");
        order.Status = command.Status.Trim();
        order.Finding = command.Finding ?? order.Finding;
        StampUpdate(order);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(order, order.OrderNumber, order.Status);
    }
}

public sealed class CreateRadiologyStudyCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), ICreateRadiologyStudyCommandHandler
{
    public async Task<CommandResult> Handle(CreateRadiologyStudyCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var study = new DiagnosticOrder
        {
            OrderNumber = command.StudyNumber.Trim(),
            Category = "Radiology",
            PatientName = command.PatientName.Trim(),
            TestName = command.Modality.Trim(),
            BodyPart = command.BodyPart.Trim(),
            Priority = "Routine",
            Status = "Requested",
            OrderedAt = DateTimeOffset.UtcNow,
            AssignedTo = command.Radiologist
        };
        StampCreate(study, tenantId);
        DbContext.DiagnosticOrders.Add(study);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(study, study.OrderNumber, study.Status);
    }
}

public sealed class UpdateRadiologyStudyStatusCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IUpdateRadiologyStudyStatusCommandHandler
{
    public async Task<CommandResult> Handle(UpdateRadiologyStudyStatusCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var study = await DbContext.DiagnosticOrders.FirstOrDefaultAsync(x => x.Id == command.StudyId && x.TenantId == tenantId && x.Category == "Radiology", cancellationToken)
            ?? throw new ValidationException("Radiology study was not found.");
        study.Status = command.Status.Trim();
        study.Finding = command.Finding ?? study.Finding;
        StampUpdate(study);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(study, study.OrderNumber, study.Status);
    }
}

public sealed class AddInventoryItemCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IAddInventoryItemCommandHandler
{
    public async Task<CommandResult> Handle(AddInventoryItemCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var item = new InventoryItem
        {
            Sku = command.Sku.Trim(),
            Name = command.Name.Trim(),
            Category = command.Category.Trim(),
            QuantityOnHand = command.QuantityOnHand,
            ReorderLevel = command.ReorderLevel,
            Unit = command.Unit.Trim(),
            ExpiryDate = command.ExpiryDate,
            Status = command.QuantityOnHand <= command.ReorderLevel ? "Low" : "In stock"
        };
        StampCreate(item, tenantId);
        DbContext.InventoryItems.Add(item);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(item, item.Sku, item.Status);
    }
}

public sealed class ReorderStockCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IReorderStockCommandHandler
{
    public async Task<CommandResult> Handle(ReorderStockCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var purchaseOrder = new PurchaseOrder
        {
            PurchaseOrderNumber = command.PurchaseOrderNumber.Trim(),
            ItemName = command.ItemName.Trim(),
            VendorName = command.VendorName.Trim(),
            Quantity = command.Quantity,
            TotalAmount = command.TotalAmount,
            ExpectedDate = command.ExpectedDate,
            Status = "Draft"
        };

        StampCreate(purchaseOrder, tenantId);
        DbContext.PurchaseOrders.Add(purchaseOrder);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(purchaseOrder, purchaseOrder.PurchaseOrderNumber, purchaseOrder.Status);
    }
}

public sealed class ScheduleTelemedicineSessionCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), IScheduleTelemedicineSessionCommandHandler
{
    public async Task<CommandResult> Handle(ScheduleTelemedicineSessionCommand command, CancellationToken cancellationToken = default)
    {
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

public sealed class CreatePurchaseOrderCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), ICreatePurchaseOrderCommandHandler
{
    public async Task<CommandResult> Handle(CreatePurchaseOrderCommand command, CancellationToken cancellationToken = default)
    {
        var tenantId = await GetTenantIdAsync(cancellationToken);
        var purchaseOrder = new PurchaseOrder
        {
            PurchaseOrderNumber = command.PurchaseOrderNumber.Trim(),
            ItemName = command.ItemName.Trim(),
            VendorName = command.VendorName.Trim(),
            Quantity = command.Quantity,
            TotalAmount = command.TotalAmount,
            ExpectedDate = command.ExpectedDate,
            Status = "Draft"
        };
        StampCreate(purchaseOrder, tenantId);
        DbContext.PurchaseOrders.Add(purchaseOrder);
        await DbContext.SaveChangesAsync(cancellationToken);
        return Result(purchaseOrder, purchaseOrder.PurchaseOrderNumber, purchaseOrder.Status);
    }
}
