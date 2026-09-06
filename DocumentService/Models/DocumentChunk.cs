namespace DocumentService.Models;

public class DocumentChunk
{
    public Guid Id { get; set; }
    public Guid KnowledgeDocumentId { get; set; }
    public KnowledgeDocument KnowledgeDocument { get; set; } = null!;
    public int ChunkIndex { get; set; }
    public string Text { get; set; } = string.Empty;
}