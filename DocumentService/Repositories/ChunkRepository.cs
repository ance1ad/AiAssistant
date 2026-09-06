using DocumentService.Application;
using DocumentService.Models;

namespace DocumentService.Repositories;

public class ChunkRepository(DocumentDbContext dbContext)
{
    public async Task Add(KnowledgeDocument knowledgeDocument)
    {
        dbContext.Add(knowledgeDocument);
        await dbContext.SaveChangesAsync();
    }

    public async Task AddChunksRange(List<DocumentChunk> chunks, CancellationToken token)
    {
        await dbContext.AddRangeAsync(chunks, token);
        await dbContext.SaveChangesAsync(token);
    }
    
}