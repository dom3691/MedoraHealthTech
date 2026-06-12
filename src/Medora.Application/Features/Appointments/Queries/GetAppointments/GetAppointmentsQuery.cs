using Medora.Application.Common.Interfaces;
using Medora.Application.Common.Models;
using Medora.Application.Features.Appointments.DTOs;

namespace Medora.Application.Features.Appointments.Queries.GetAppointments;

public sealed record GetAppointmentsQuery(QueryParameters Parameters, string? Department = null, string? Status = null)
    : IQuery<PagedResult<AppointmentDto>>;
