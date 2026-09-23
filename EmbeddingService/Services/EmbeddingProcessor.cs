using EmbeddingService.Abstractions;
using EmbeddingService.Models;
using EmbeddingService.Repositories;
using Pgvector;
using Shared.Contracts.Events;

namespace EmbeddingService.Services;

public class EmbeddingProcessor(
    EmbeddingRepository repository, 
    IEmbeddingService embeddingService,
    ILogger<EmbeddingProcessor> logger)
{
    public async Task AddVectors(
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
    
    public async Task<List<SimilarityResult>> FindSimilarities(string question)
    {
        var embeddingQuestionResult = await embeddingService.CreateEmbeddings(new List<string>
        {
            question
        });
        
        Vector vector = embeddingQuestionResult[0];

        var similarityResults = await repository.FindSimilarities(vector);
        
        logger.LogInformation($"Found {similarityResults.Count} similarity results");
        
        return similarityResults;
    }
}