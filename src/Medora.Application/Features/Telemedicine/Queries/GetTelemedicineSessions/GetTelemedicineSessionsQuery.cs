using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.Telemedicine.Queries.GetTelemedicineSessions;

public sealed record GetTelemedicineSessionsQuery(string? TenantCode) : IQuery<object>;

public interface IGetTelemedicineSessionsQueryHandler : IQueryHandler<GetTelemedicineSessionsQuery, object>;
