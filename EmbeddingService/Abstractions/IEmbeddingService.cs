using Pgvector;

namespace EmbeddingService.Abstractions;

public interface IEmbeddingService
{
    public Task<Vector> CreateEmbedding(string text, CancellationToken cancellationToken = default);
}