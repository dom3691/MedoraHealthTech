using Medora.Application.Common.Interfaces;

namespace Medora.Application.Features.Nursing.Queries.GetNursingTasks;

public sealed record GetNursingTasksQuery(string? TenantCode) : IQuery<object>;

public interface IGetNursingTasksQueryHandler : IQueryHandler<GetNursingTasksQuery, object>;
