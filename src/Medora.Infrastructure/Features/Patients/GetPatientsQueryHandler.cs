using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;
using Medora.Application.Features.Patients.DTOs;
using Medora.Application.Features.Patients.Queries.GetPatients;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.Features.Patients;

public sealed class GetPatientsQueryHandler(
    MedoraDbContext dbContext,
    ICurrentTenantService currentTenant) : IGetPatientsQueryHandler
{
    public async Task<PagedResult<PatientDto>> Handle(GetPatientsQuery query, CancellationToken cancellationToken = default)
    {
        var tenantId = currentTenant.TenantId ?? await ResolveFallbackTenantIdAsync(cancellationToken);
        var parameters = query.Parameters;

        var patientsQuery = dbContext.Patients
            .AsNoTracking()
            .Where(x => x.TenantId == tenantId && !x.IsDeleted);

        if (!string.IsNullOrWhiteSpace(parameters.Search))
        {
            var search = parameters.Search.Trim();
            patientsQuery = patientsQuery.Where(x => x.FullName.Contains(search) || x.Mrn.Contains(search));
        }

        patientsQuery = (parameters.SortBy?.ToLowerInvariant(), parameters.SortDirection?.ToLowerInvariant()) switch
        {
            ("mrn", "desc") => patientsQuery.OrderByDescending(x => x.Mrn),
            ("mrn", _) => patientsQuery.OrderBy(x => x.Mrn),
            ("status", "desc") => patientsQuery.OrderByDescending(x => x.Status),
            ("status", _) => patientsQuery.OrderBy(x => x.Status),
            ("name", "desc") => patientsQuery.OrderByDescending(x => x.FullName),
            _ => patientsQuery.OrderBy(x => x.FullName)
        };

        var totalCount = await patientsQuery.CountAsync(cancellationToken);
        var items = await patientsQuery
            .Skip((parameters.SafePageNumber - 1) * parameters.SafePageSize)
            .Take(parameters.SafePageSize)
            .Select(x => new PatientDto(
                x.Id,
                x.Mrn,
                x.FullName,
                x.DateOfBirth.HasValue ? CalculateAge(x.DateOfBirth.Value) : 0,
                x.Sex ?? string.Empty,
                x.Status ?? "Unknown",
                null,
                null,
                null))
            .ToListAsync(cancellationToken);

        return new PagedResult<PatientDto>(items, parameters.SafePageNumber, parameters.SafePageSize, totalCount);
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

    private static int CalculateAge(DateOnly dateOfBirth)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var age = today.Year - dateOfBirth.Year;
        return dateOfBirth > today.AddYears(-age) ? age - 1 : age;
    }
}
