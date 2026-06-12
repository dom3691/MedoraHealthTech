using Medora.Application.Abstractions;
using Medora.Application.Modules.Identity.Auth;
using Medora.Application.Modules.Operations.Data;
using Medora.Application.Modules.Platform.Tenants;
using Medora.Application.Modules.TenantAdministration.Users;
using Medora.Application.Features.Appointments.Queries.GetAppointments;
using Medora.Application.Features.Patients.Queries.GetPatients;
using Medora.Infrastructure.Features.Appointments;
using Medora.Infrastructure.Features.Patients;
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
