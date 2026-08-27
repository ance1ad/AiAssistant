using Microsoft.EntityFrameworkCore;
using WebApplication1.Application;
using WebApplication1.Models;

namespace WebApplication1.Repositories;

public class DocumentRepository(AssistentDbContext dbContext)
{
    public async Task Add(KnowledgeDocument knowledgeDocument)
    {
        dbContext.Add(knowledgeDocument);
        await dbContext.SaveChangesAsync();
    }

    public async Task<KnowledgeDocument?> GetPendingDocument()
    {
        var doc = await dbContext.Documents
            .FirstOrDefaultAsync(d => d.Status == DocumentStatus.Pending);
        
        return doc;
    }

    public async Task SetDocProcessing(Guid id)
    {
        await dbContext.Documents
            .Where(d => d.Id == id)
            .ExecuteUpdateAsync(d => d
                .SetProperty(doc => doc.Status, DocumentStatus.Processing)
            );
    }
}