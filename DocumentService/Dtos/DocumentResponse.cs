using Shared.Contracts.Events;
using Shared.Contracts.Models;

namespace DocumentService.Dtos;

public record DocumentResponse(
    Guid Id,
    string FileName,
    ProcessingStatus Status
);