using DocumentService.Models;

namespace DocumentService.Dtos.Document;

public record DocumentResponse(
    Guid Id,
    string FileName,
    DocumentStatus Status
);