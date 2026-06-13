using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.Staff.Queries.GetStaff;

public sealed record GetStaffQuery(string? TenantCode) : IQuery<object>;

public interface IGetStaffQueryHandler : IQueryHandler<GetStaffQuery, object>;
