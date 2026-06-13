using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.WardsAndBeds.Queries.GetBeds;

public sealed record GetBedsQuery(string? TenantCode) : IQuery<object>;

public interface IGetBedsQueryHandler : IQueryHandler<GetBedsQuery, object>;
