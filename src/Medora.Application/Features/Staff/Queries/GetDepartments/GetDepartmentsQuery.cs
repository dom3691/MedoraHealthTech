using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.Staff.Queries.GetDepartments;

public sealed record GetDepartmentsQuery(string? TenantCode) : IQuery<object>;

public interface IGetDepartmentsQueryHandler : IQueryHandler<GetDepartmentsQuery, object>;
