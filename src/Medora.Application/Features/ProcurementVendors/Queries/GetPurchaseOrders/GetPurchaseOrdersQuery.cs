using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.ProcurementVendors.Queries.GetPurchaseOrders;

public sealed record GetPurchaseOrdersQuery(string? TenantCode) : IQuery<object>;

public interface IGetPurchaseOrdersQueryHandler : IQueryHandler<GetPurchaseOrdersQuery, object>;
