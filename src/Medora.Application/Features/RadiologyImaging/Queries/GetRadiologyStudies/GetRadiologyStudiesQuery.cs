using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.RadiologyImaging.Queries.GetRadiologyStudies;

public sealed record GetRadiologyStudiesQuery(string? TenantCode) : IQuery<object>;

public interface IGetRadiologyStudiesQueryHandler : IQueryHandler<GetRadiologyStudiesQuery, object>;
