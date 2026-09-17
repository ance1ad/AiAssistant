using Pgvector;

namespace EmbeddingService.Abstractions;

public interface IEmbeddingService
{
    public Task<IReadOnlyList<Vector>> CreateEmbeddings(
        IReadOnlyList<string> texts, 
        CancellationToken cancellationToken = default);
}