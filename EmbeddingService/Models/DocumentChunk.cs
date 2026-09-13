using Pgvector;

namespace EmbeddingService.Models;

public class DocumentChunk
{
    public Guid Id { get; set; }
    public Guid KnowledgeDocumentId { get; set; }
    public int ChunkIndex { get; set; }
    public string Text { get; set; } = string.Empty;
    public Vector? Embedding { get; set; }
}