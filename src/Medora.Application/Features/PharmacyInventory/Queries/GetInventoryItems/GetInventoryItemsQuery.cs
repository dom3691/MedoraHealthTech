using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.PharmacyInventory.Queries.GetInventoryItems;

public sealed record GetInventoryItemsQuery(string? TenantCode) : IQuery<object>;

public interface IGetInventoryItemsQueryHandler : IQueryHandler<GetInventoryItemsQuery, object>;
