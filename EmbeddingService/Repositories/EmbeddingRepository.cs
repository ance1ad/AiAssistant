using EmbeddingService.Application;
using EmbeddingService.Models;
using Microsoft.EntityFrameworkCore;
using Pgvector;
using Pgvector.EntityFrameworkCore;
using Shared.Contracts.Events;

namespace EmbeddingService.Repositories;

public class EmbeddingRepository(EmbeddingDbContext context)
{
    public async Task AddRange(List<Embedding> embeddingRecords)
    {
        await context.Embeddings.AddRangeAsync(embeddingRecords);
        await context.SaveChangesAsync();
    }

    public async Task<List<SimilarityResult>> FindSimilarities(Vector questionVector)
    {
        var results = await context.Embeddings
            .OrderBy(e => e.Vector!.CosineDistance(questionVector))
            .Take(5)
            .Select(e => new SimilarityResult
                (
                    e.ResourceId,
                    e.SourceType,
                    1 - e.Vector!.CosineDistance(questionVector)
            ))
            .ToListAsync();
        return results;
    }
}