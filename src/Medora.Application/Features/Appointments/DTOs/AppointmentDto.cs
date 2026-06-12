namespace Medora.Application.Features.Appointments.DTOs;

public sealed record AppointmentDto(
    Guid Id,
    string AppointmentNumber,
    string PatientName,
    string Provider,
    string Department,
    string VisitType,
    string Room,
    string Status,
    DateTimeOffset ScheduledAt);
