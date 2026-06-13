using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.WardsAndBeds.Queries.GetWards;

public sealed record GetWardsQuery(string? TenantCode) : IQuery<object>;

public interface IGetWardsQueryHandler : IQueryHandler<GetWardsQuery, object>;
