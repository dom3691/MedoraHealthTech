using Medora.Domain.Common;
using Medora.Domain.Entities;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.Features.Common;

public abstract class TenantReadQueryHandlerBase(MedoraDbContext dbContext)
{
    protected MedoraDbContext DbContext { get; } = dbContext;

    protected async Task<object> TenantSetAsync<TEntity>(string? tenantCode, IQueryable<TEntity> query, CancellationToken cancellationToken)
        where TEntity : TenantScopedEntity
    {
        var tenant = await ResolveTenantAsync(tenantCode, cancellationToken);
        return await query
            .AsNoTracking()
            .Where(x => x.TenantId == tenant.Id && !x.IsDeleted)
            .ToListAsync(cancellationToken);
    }

    protected async Task<Tenant> ResolveTenantAsync(string? tenantCode, CancellationToken cancellationToken)
    {
        if (!string.IsNullOrWhiteSpace(tenantCode))
        {
            var normalized = tenantCode.Trim();
            var tenant = await DbContext.Tenants
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Code == normalized && x.IsActive, cancellationToken);

            if (tenant is not null)
            {
                return tenant;
            }
        }

        return await DbContext.Tenants
            .AsNoTracking()
            .OrderBy(x => x.Code)
            .FirstAsync(x => x.IsActive, cancellationToken);
    }
}
