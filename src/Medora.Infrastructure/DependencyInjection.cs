using Medora.Application.Abstractions;
using Medora.Application.Common.Interfaces;
using Medora.Application.Modules.Identity.Auth;
using Medora.Application.Modules.Platform.Tenants;
using Medora.Application.Modules.TenantAdministration.Users;
using Medora.Application.Features.Appointments.Queries.GetAppointments;
using Medora.Infrastructure.Features.WardsAndBeds;
using Medora.Infrastructure.Features.TheatreAndSurgery;
using Medora.Infrastructure.Features.Tenants;
using Medora.Infrastructure.Features.Telemedicine;
using Medora.Infrastructure.Features.RadiologyImaging;
using Medora.Infrastructure.Features.ProcurementVendors;
using Medora.Infrastructure.Features.PharmacyInventory;
using Medora.Infrastructure.Features.PatientFlow;
using Medora.Infrastructure.Features.Outpatient;
using Medora.Infrastructure.Features.Nursing;
using Medora.Infrastructure.Features.LabDiagnostics;
using Medora.Infrastructure.Features.Inpatient;
using Medora.Infrastructure.Features.Emergency;
using Medora.Application.Features.WardsAndBeds.Queries.GetWards;
using Medora.Application.Features.WardsAndBeds.Queries.GetBeds;
using Medora.Application.Features.TheatreAndSurgery.Queries.GetSurgeryCases;
using Medora.Application.Features.Tenants.Queries.GetTenants;
using Medora.Application.Features.Telemedicine.Queries.GetTelemedicineSessions;
using Medora.Application.Features.Staff.Queries.GetStaff;
using Medora.Application.Features.Staff.Queries.GetDepartments;
using Medora.Application.Features.RadiologyImaging.Queries.GetRadiologyStudies;
using Medora.Application.Features.ProcurementVendors.Queries.GetVendors;
using Medora.Application.Features.ProcurementVendors.Queries.GetPurchaseOrders;
using Medora.Application.Features.PharmacyInventory.Queries.GetInventoryItems;
using Medora.Application.Features.PatientFlow.Queries.GetPatientEncounters;
using Medora.Application.Features.Outpatient.Queries.GetOutpatientVisits;
using Medora.Application.Features.Nursing.Queries.GetNursingTasks;
using Medora.Application.Features.Notifications.Queries.GetNotifications;
using Medora.Application.Features.LabDiagnostics.Queries.GetLabOrders;
using Medora.Application.Features.Inpatient.Queries.GetAdmissions;
using Medora.Application.Features.Emergency.Queries.GetEmergencyCases;
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
using Medora.Infrastructure.Features.Notifications;
using Medora.Infrastructure.Features.Patients;
using Medora.Infrastructure.Features.Staff;
using Medora.Infrastructure.Identity;
using Medora.Application.Screens;
using Medora.Infrastructure.Persistence;
using Medora.Infrastructure.ReadModels;
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
        services.AddScoped<IGetTenantsQueryHandler, GetTenantsQueryHandler>();
        services.AddScoped<IGetPatientEncountersQueryHandler, GetPatientEncountersQueryHandler>();
        services.AddScoped<IGetOutpatientVisitsQueryHandler, GetOutpatientVisitsQueryHandler>();
        services.AddScoped<IGetAdmissionsQueryHandler, GetAdmissionsQueryHandler>();
        services.AddScoped<IGetEmergencyCasesQueryHandler, GetEmergencyCasesQueryHandler>();
        services.AddScoped<IGetWardsQueryHandler, GetWardsQueryHandler>();
        services.AddScoped<IGetBedsQueryHandler, GetBedsQueryHandler>();
        services.AddScoped<IGetSurgeryCasesQueryHandler, GetSurgeryCasesQueryHandler>();
        services.AddScoped<IGetNursingTasksQueryHandler, GetNursingTasksQueryHandler>();
        services.AddScoped<IGetLabOrdersQueryHandler, GetLabOrdersQueryHandler>();
        services.AddScoped<IGetRadiologyStudiesQueryHandler, GetRadiologyStudiesQueryHandler>();
        services.AddScoped<IGetInventoryItemsQueryHandler, GetInventoryItemsQueryHandler>();
        services.AddScoped<IGetVendorsQueryHandler, GetVendorsQueryHandler>();
        services.AddScoped<IGetPurchaseOrdersQueryHandler, GetPurchaseOrdersQueryHandler>();
        services.AddScoped<IGetNotificationsQueryHandler, GetNotificationsQueryHandler>();
        services.AddScoped<IGetStaffQueryHandler, GetStaffQueryHandler>();
        services.AddScoped<IGetDepartmentsQueryHandler, GetDepartmentsQueryHandler>();
        services.AddScoped<IGetTelemedicineSessionsQueryHandler, GetTelemedicineSessionsQueryHandler>();
        services.AddScoped<ITenantReadModelService, TenantReadModelService>();
        services.AddScoped<IPlatformTenantOnboardingService, TenantOnboardingService>();
        services.AddScoped<ITenantUserOnboardingService, TenantUserOnboardingService>();
        services.AddSingleton<IDateTimeProvider, SystemDateTimeProvider>();
        services.AddScoped<IScreenCatalog, EfScreenCatalog>();
        return services;
    }
}





