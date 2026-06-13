using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.LabDiagnostics.Queries.GetLabOrders;

public sealed record GetLabOrdersQuery(string? TenantCode) : IQuery<object>;

public interface IGetLabOrdersQueryHandler : IQueryHandler<GetLabOrdersQuery, object>;
