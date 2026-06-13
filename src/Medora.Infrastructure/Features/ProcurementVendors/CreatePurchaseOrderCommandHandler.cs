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


namespace Medora.Infrastructure.Features.ProcurementVendors;

public sealed class CreatePurchaseOrderCommandHandler(MedoraDbContext dbContext, ICurrentTenantService currentTenant, ICurrentUserService currentUser)
    : TenantCommandHandlerBase(dbContext, currentTenant, currentUser), ICreatePurchaseOrderCommandHandler
{
    public async Task<CommandResult> Handle(CreatePurchaseOrderCommand command, CancellationToken cancellationToken = default)
    {
        RequireText(command.PurchaseOrderNumber, "Purchase order number");
        RequireText(command.ItemName, "Item name");
        RequireText(command.VendorName, "Vendor name");
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

