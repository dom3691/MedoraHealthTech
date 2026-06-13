using Medora.Application.Common.Exceptions;
using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;
using Medora.Domain.Common;
using Medora.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Medora.Infrastructure.Features.Common;

public abstract class TenantCommandHandlerBase(
    MedoraDbContext dbContext,
    ICurrentTenantService currentTenant,
    ICurrentUserService currentUser)
{
    protected MedoraDbContext DbContext { get; } = dbContext;

    protected async Task<Guid> GetTenantIdAsync(CancellationToken cancellationToken)
    {
        if (currentTenant.TenantId.HasValue)
        {
            return currentTenant.TenantId.Value;
        }

        return await DbContext.Tenants
            .AsNoTracking()
            .Where(x => x.IsActive && x.Code != "MEDORA")
            .OrderBy(x => x.Code)
            .Select(x => x.Id)
            .FirstAsync(cancellationToken);
    }

    protected void StampCreate(TenantScopedEntity entity, Guid tenantId)
    {
        entity.TenantId = tenantId;
        entity.CreatedAt = DateTimeOffset.UtcNow;
        entity.CreatedBy = currentUser.Email;
    }

    protected void StampUpdate(TenantScopedEntity entity)
    {
        entity.UpdatedAt = DateTimeOffset.UtcNow;
        entity.UpdatedBy = currentUser.Email;
    }

    protected static void Require(bool condition, string message)
    {
        if (!condition)
        {
            throw new ValidationException(message);
        }
    }

    protected static void RequireText(string? value, string fieldName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ValidationException($"{fieldName} is required.");
        }
    }

    protected static CommandResult Result(TenantScopedEntity entity, string reference, string status) =>
        new(entity.Id, reference, status);
}
