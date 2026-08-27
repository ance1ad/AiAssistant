namespace WebApplication1.Models;

public class KnowledgeDocument
{
    public Guid Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string ContentType { get; set; } = string.Empty;
    public DocumentStatus Status { get; set; } 
    public DateTime CreatedAt { get; set; } 
    public DateTime? ProcessedAt { get; set; } 
    public string? ErrorMessage { get; set; } 
}

public enum DocumentStatus
{
    Pending,
    Processing,
    Error,
    Complete
}