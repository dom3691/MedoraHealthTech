using Medora.Application.Features.Tenants.Queries.GetTenants;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.Features.Tenants;

public sealed class GetTenantsQueryHandler(MedoraDbContext dbContext) : IGetTenantsQueryHandler
{
    public async Task<object> Handle(GetTenantsQuery query, CancellationToken cancellationToken = default)
    {
        return await dbContext.Tenants
            .AsNoTracking()
            .Where(x => x.IsActive)
            .OrderBy(x => x.Code)
            .ToListAsync(cancellationToken);
    }
}
