using DocumentService.Dtos;
using EmbeddingService.Abstractions;
using EmbeddingService.Models;
using EmbeddingService.Repositories;
using Pgvector;
using Shared.Contracts.Events;
using Shared.Messaging;

namespace EmbeddingService.Services;

public class EmbeddingCreator(
    EmbeddingRepository repository, 
    IEmbeddingService embeddingService)
{
    public async Task CreateVector(
        TextChunksPreparedEvent chunks)
    {
        var texts = chunks.Chunks
            .Select(chunk => chunk.Text)
            .ToList();
        
        var vectors = await embeddingService.CreateEmbeddings(texts);
        
        var embeddings = vectors
            .Select((vector, index) => new Embedding
            {
                Id = Guid.NewGuid(), 
                ResourceId = chunks.Chunks[index].Id, 
                SourceType = chunks.SourceType, 
                Vector = vector
            }).ToList();
        
        await repository.AddRange(embeddings);
    }
}