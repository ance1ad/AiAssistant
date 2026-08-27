using WebApplication1.Models;

namespace WebApplication1.Dtos.Document;

public record DocumentResponse(
    Guid Id,
    string FileName,
    DocumentStatus Status
);