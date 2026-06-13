using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.ProcurementVendors.Queries.GetVendors;

public sealed record GetVendorsQuery(string? TenantCode) : IQuery<object>;

public interface IGetVendorsQueryHandler : IQueryHandler<GetVendorsQuery, object>;
