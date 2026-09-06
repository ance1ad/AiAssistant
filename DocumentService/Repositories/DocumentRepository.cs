using DocumentService.Application;
using DocumentService.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Repositories;

public class DocumentRepository(DocumentDbContext dbContext)
{
    public async Task Add(KnowledgeDocument knowledgeDocument)
    {
        dbContext.Add(knowledgeDocument);
        await dbContext.SaveChangesAsync();
    }

    public async Task<KnowledgeDocument?> GetPendingDocument(CancellationToken token)
    {
        var doc = await dbContext.Documents
            .FirstOrDefaultAsync(
                d => d.Status == DocumentStatus.Pending, 
                cancellationToken: token
            );
        
        return doc;
    }

    public async Task SetDocumentProcessing(Guid id, CancellationToken token)
    {
        await dbContext.Documents
            .Where(d => d.Id == id)
            .ExecuteUpdateAsync(d => d
                .SetProperty(doc => doc.Status, DocumentStatus.Processing), cancellationToken: token);
    }
    
    public async Task SetDocumentComplete(Guid id, string text, CancellationToken token)
    {
        await dbContext.Documents
            .Where(d => d.Id == id)
            .ExecuteUpdateAsync(d => d
                .SetProperty(doc => doc.Status, DocumentStatus.Complete)
                .SetProperty(doc => doc.ProcessedAt, DateTime.UtcNow)
                .SetProperty(doc => doc.Text, text), cancellationToken: token);
    }
    
    public async Task SetDocumentError(Guid id, string error, CancellationToken token)
    {
        await dbContext.Documents
            .Where(d => d.Id == id)
            .ExecuteUpdateAsync(d => d
                .SetProperty(doc => doc.Status, DocumentStatus.Error)
                .SetProperty(doc => doc.ErrorMessage, error)
                .SetProperty(doc => doc.ProcessedAt, DateTime.UtcNow), cancellationToken: token);
    }
}