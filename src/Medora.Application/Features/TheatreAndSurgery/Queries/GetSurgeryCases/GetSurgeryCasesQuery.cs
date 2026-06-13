using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.TheatreAndSurgery.Queries.GetSurgeryCases;

public sealed record GetSurgeryCasesQuery(string? TenantCode) : IQuery<object>;

public interface IGetSurgeryCasesQueryHandler : IQueryHandler<GetSurgeryCasesQuery, object>;
