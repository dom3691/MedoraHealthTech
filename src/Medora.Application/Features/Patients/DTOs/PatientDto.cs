namespace Medora.Application.Features.Patients.DTOs;

public sealed record PatientDto(
    Guid Id,
    string Mrn,
    string FullName,
    int Age,
    string Sex,
    string Status,
    string? Ward,
    string? AttendingProvider,
    DateOnly? AdmittedOn);
