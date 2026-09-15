using EmbeddingService.Abstractions;
using EmbeddingService.Models;
using EmbeddingService.Repositories;
using Pgvector;
using Shared.Contracts.Events;

namespace EmbeddingService.Services;

public class EmbeddingCreator(
    EmbeddingRepository repository, 
    IEmbeddingService embeddingService)
{
    public async Task CreateVector(DocumentChunksCreatedEvent chunks)
    {
        var embeddings = new List<Embedding>();
        foreach (var chunk in chunks.Chunks)
        {
            var vector = await embeddingService.CreateEmbedding(chunk.Text);
            embeddings.Add(new Embedding {
                Id = Guid.NewGuid(),
                ResourceId = chunk.Id,
                SourceType = chunks.SourceType,
                Vector = vector}
            );
        }
        
        await repository.CreateEmbeddings(embeddings);
    }
}