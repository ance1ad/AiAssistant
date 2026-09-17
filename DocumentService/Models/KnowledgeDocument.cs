using Shared.Contracts.Events;

namespace DocumentService.Models;

public class KnowledgeDocument
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public string? Text { get; set; }
    public ProcessingStatus Status { get; set; } 
    public DateTime CreatedAt { get; set; } 
    public DateTime? ProcessedAt { get; set; } 
    public string? ErrorMessage { get; set; }
    public List<DocumentChunk> Chunks { get; set; } = [];
}