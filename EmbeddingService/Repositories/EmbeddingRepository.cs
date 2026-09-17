using EmbeddingService.Application;
using EmbeddingService.Models;

namespace EmbeddingService.Repositories;

public class EmbeddingRepository(EmbeddingDbContext context)
{
    public async Task AddRange(List<Embedding> embeddingRecords)
    {
        await context.Embeddings.AddRangeAsync(embeddingRecords);
        await context.SaveChangesAsync();
    }
}