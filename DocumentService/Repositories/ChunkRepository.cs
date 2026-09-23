using DocumentService.Application;
using DocumentService.Models;
using Microsoft.EntityFrameworkCore;

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
    
    public async Task<DocumentChunk?> Get(Guid id)
    {
        return await dbContext.Chunks.Where(a => a.Id == id)
            .AsNoTracking()
            .FirstOrDefaultAsync();
    }
    
}