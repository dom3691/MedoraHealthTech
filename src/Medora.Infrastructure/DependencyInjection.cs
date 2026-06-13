using Medora.Application.Abstractions;
using Medora.Application.Modules.Identity.Auth;
using Medora.Application.Modules.Operations.Data;
using Medora.Application.Modules.Platform.Tenants;
using Medora.Application.Modules.TenantAdministration.Users;
using Medora.Application.Features.Appointments.Queries.GetAppointments;
using Medora.Application.Features.Appointments.Commands.CreateAppointment;
using Medora.Application.Features.Appointments.Commands.UpdateAppointmentStatus;
using Medora.Application.Features.Emergency.Commands.RegisterEmergencyArrival;
using Medora.Application.Features.Emergency.Commands.UpdateEmergencyStatus;
using Medora.Application.Features.Inpatient.Commands.AdmitPatient;
using Medora.Application.Features.Inpatient.Commands.DischargePatient;
using Medora.Application.Features.LabDiagnostics.Commands.CreateLabOrder;
using Medora.Application.Features.LabDiagnostics.Commands.UpdateLabOrderStatus;
using Medora.Application.Features.Notifications.Commands.CreateNotification;
using Medora.Application.Features.Nursing.Commands.CreateNursingTask;
using Medora.Application.Features.Nursing.Commands.UpdateNursingTaskStatus;
using Medora.Application.Features.Outpatient.Commands.BookOutpatientVisit;
using Medora.Application.Features.PatientFlow.Commands.MovePatientFlowStage;
using Medora.Application.Features.PatientFlow.Commands.RegisterWalkIn;
using Medora.Application.Features.Patients.Queries.GetPatients;
using Medora.Application.Features.Patients.Commands.CreatePatient;
using Medora.Application.Features.PharmacyInventory.Commands.AddInventoryItem;
using Medora.Application.Features.PharmacyInventory.Commands.ReorderStock;
using Medora.Application.Features.ProcurementVendors.Commands.CreatePurchaseOrder;
using Medora.Application.Features.RadiologyImaging.Commands.CreateRadiologyStudy;
using Medora.Application.Features.RadiologyImaging.Commands.UpdateRadiologyStudyStatus;
using Medora.Application.Features.Staff.Commands.AddStaffMember;
using Medora.Application.Features.Telemedicine.Commands.ScheduleTelemedicineSession;
using Medora.Application.Features.TheatreAndSurgery.Commands.ScheduleSurgery;
using Medora.Application.Features.TheatreAndSurgery.Commands.UpdateSurgeryStatus;
using Medora.Application.Features.WardsAndBeds.Commands.AllocateBed;
using Medora.Application.Features.WardsAndBeds.Commands.UpdateBedStatus;
using Medora.Infrastructure.Features.Appointments;
using Medora.Infrastructure.Features.HospitalOperations;
using Medora.Infrastructure.Features.Notifications;
using Medora.Infrastructure.Features.Patients;
using Medora.Infrastructure.Features.Staff;
using Medora.Infrastructure.Identity;
using Medora.Application.Screens;
using Medora.Infrastructure.Operations;
using Medora.Infrastructure.Persistence;
using Medora.Infrastructure.Persistence.Seed;
using Medora.Infrastructure.Screens;
using Medora.Infrastructure.TenantAdministration;
using Medora.Infrastructure.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Medora.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MedoraDb")
            ?? throw new InvalidOperationException("Connection string 'MedoraDb' is not configured.");

        services.AddDbContext<MedoraDbContext>(options => options.UseSqlServer(connectionString));
        services.AddScoped<DatabaseSeeder>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ICreatePatientCommandHandler, CreatePatientCommandHandler>();
        services.AddScoped<ICreateAppointmentCommandHandler, CreateAppointmentCommandHandler>();
        services.AddScoped<IUpdateAppointmentStatusCommandHandler, UpdateAppointmentStatusCommandHandler>();
        services.AddScoped<IRegisterWalkInCommandHandler, RegisterWalkInCommandHandler>();
        services.AddScoped<IMovePatientFlowStageCommandHandler, MovePatientFlowStageCommandHandler>();
        services.AddScoped<IBookOutpatientVisitCommandHandler, BookOutpatientVisitCommandHandler>();
        services.AddScoped<IAdmitPatientCommandHandler, AdmitPatientCommandHandler>();
        services.AddScoped<IDischargePatientCommandHandler, DischargePatientCommandHandler>();
        services.AddScoped<IRegisterEmergencyArrivalCommandHandler, RegisterEmergencyArrivalCommandHandler>();
        services.AddScoped<IUpdateEmergencyStatusCommandHandler, UpdateEmergencyStatusCommandHandler>();
        services.AddScoped<IAllocateBedCommandHandler, AllocateBedCommandHandler>();
        services.AddScoped<IUpdateBedStatusCommandHandler, UpdateBedStatusCommandHandler>();
        services.AddScoped<IScheduleSurgeryCommandHandler, ScheduleSurgeryCommandHandler>();
        services.AddScoped<IUpdateSurgeryStatusCommandHandler, UpdateSurgeryStatusCommandHandler>();
        services.AddScoped<ICreateNursingTaskCommandHandler, CreateNursingTaskCommandHandler>();
        services.AddScoped<IUpdateNursingTaskStatusCommandHandler, UpdateNursingTaskStatusCommandHandler>();
        services.AddScoped<ICreateLabOrderCommandHandler, CreateLabOrderCommandHandler>();
        services.AddScoped<IUpdateLabOrderStatusCommandHandler, UpdateLabOrderStatusCommandHandler>();
        services.AddScoped<ICreateRadiologyStudyCommandHandler, CreateRadiologyStudyCommandHandler>();
        services.AddScoped<IUpdateRadiologyStudyStatusCommandHandler, UpdateRadiologyStudyStatusCommandHandler>();
        services.AddScoped<IAddInventoryItemCommandHandler, AddInventoryItemCommandHandler>();
        services.AddScoped<IReorderStockCommandHandler, ReorderStockCommandHandler>();
        services.AddScoped<IScheduleTelemedicineSessionCommandHandler, ScheduleTelemedicineSessionCommandHandler>();
        services.AddScoped<ICreatePurchaseOrderCommandHandler, CreatePurchaseOrderCommandHandler>();
        services.AddScoped<ICreateNotificationCommandHandler, CreateNotificationCommandHandler>();
        services.AddScoped<IAddStaffMemberCommandHandler, AddStaffMemberCommandHandler>();
        services.AddScoped<IGetAppointmentsQueryHandler, GetAppointmentsQueryHandler>();
        services.AddScoped<IGetPatientsQueryHandler, GetPatientsQueryHandler>();
        services.AddScoped<IOperationsDataQueryService, OperationsDataQueryService>();
        services.AddScoped<IPlatformTenantOnboardingService, TenantOnboardingService>();
        services.AddScoped<ITenantUserOnboardingService, TenantUserOnboardingService>();
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddScoped<IScreenCatalog, EfScreenCatalog>();
        return services;
    }
}
