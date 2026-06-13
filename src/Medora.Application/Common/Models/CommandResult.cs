namespace Medora.Application.Common.Models;

public sealed record CommandResult(Guid Id, string Reference, string Status);
