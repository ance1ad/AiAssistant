using Pgvector;
using Shared.Contracts.Events;

namespace EmbeddingService.Models;

public class Embedding
{
    public Guid Id { get; set; }
    public Guid ResourceId { get; set; }
    public SourceType SourceType { get; set; }
    public Vector? Vector { get; set; }
}