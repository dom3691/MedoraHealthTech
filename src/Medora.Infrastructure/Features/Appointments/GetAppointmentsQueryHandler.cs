using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;
using Medora.Application.Features.Appointments.DTOs;
using Medora.Application.Features.Appointments.Queries.GetAppointments;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.Features.Appointments;

public sealed class GetAppointmentsQueryHandler(
    MedoraDbContext dbContext,
    ICurrentTenantService currentTenant) : IGetAppointmentsQueryHandler
{
    public async Task<PagedResult<AppointmentDto>> Handle(GetAppointmentsQuery query, CancellationToken cancellationToken = default)
    {
        var tenantId = currentTenant.TenantId ?? await ResolveFallbackTenantIdAsync(cancellationToken);
        var parameters = query.Parameters;

        var appointmentsQuery = dbContext.Appointments
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            var search = parameters.Search.Trim();
            appointmentsQuery = appointmentsQuery.Where(x =>
                x.PatientName.Contains(search) ||
                x.ProviderName.Contains(search) ||
                x.AppointmentNumber.Contains(search));
        }

        if (!string.IsNullOrWhiteSpace(query.Department))
        {
            appointmentsQuery = appointmentsQuery.Where(x => x.Department == query.Department);
        }

        if (!string.IsNullOrWhiteSpace(query.Status))
        {
            appointmentsQuery = appointmentsQuery.Where(x => x.Status == query.Status);
        }

        appointmentsQuery = (parameters.SortBy?.ToLowerInvariant(), parameters.SortDirection?.ToLowerInvariant()) switch
        {
            ("patient", "desc") => appointmentsQuery.OrderByDescending(x => x.PatientName),
            ("patient", _) => appointmentsQuery.OrderBy(x => x.PatientName),
            ("provider", "desc") => appointmentsQuery.OrderByDescending(x => x.ProviderName),
            ("provider", _) => appointmentsQuery.OrderBy(x => x.ProviderName),
            ("status", "desc") => appointmentsQuery.OrderByDescending(x => x.Status),
            ("status", _) => appointmentsQuery.OrderBy(x => x.Status),
            ("scheduledat", "desc") => appointmentsQuery.OrderByDescending(x => x.ScheduledAt),
            _ => appointmentsQuery.OrderBy(x => x.ScheduledAt)
        };

        var totalCount = await appointmentsQuery.CountAsync(cancellationToken);
        var items = await appointmentsQuery
            .Skip((parameters.SafePageNumber - 1) * parameters.SafePageSize)
            .Take(parameters.SafePageSize)
            .Select(x => new AppointmentDto(
                x.Id,
                x.AppointmentNumber,
                x.PatientName,
                x.ProviderName,
                x.Department,
                x.VisitType,
                x.Room ?? string.Empty,
                x.Status,
                x.ScheduledAt))
            .ToListAsync(cancellationToken);

        return new PagedResult<AppointmentDto>(items, parameters.SafePageNumber, parameters.SafePageSize, totalCount);
    }

    private async Task<Guid> ResolveFallbackTenantIdAsync(CancellationToken cancellationToken)
    {
        return await dbContext.Tenants
            .AsNoTracking()
            .Where(x => x.IsActive && x.Code != "MEDORA")
            .OrderBy(x => x.Code)
            .Select(x => x.Id)
            .FirstAsync(cancellationToken);
    }
}
