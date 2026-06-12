using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;
using Medora.Application.Features.Appointments.DTOs;

namespace Medora.Application.Features.Appointments.Queries.GetAppointments;

public interface IGetAppointmentsQueryHandler : IQueryHandler<GetAppointmentsQuery, PagedResult<AppointmentDto>>
{
}
