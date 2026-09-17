using DocumentService.Application;
using DocumentService.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Contracts.Events;

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
                d => d.Status == ProcessingStatus.Pending, 
                cancellationToken: token
            );
        
        return doc;
    }

    public async Task SetDocumentProcessing(Guid id, CancellationToken token)
    {
        await dbContext.Documents
            .Where(d => d.Id == id)
            .ExecuteUpdateAsync(d => d
                .SetProperty(doc => doc.Status, ProcessingStatus.Processing), cancellationToken: token);
    }
    
    public async Task SetDocumentProcessed(Guid id, string text, CancellationToken token)
    {
        await dbContext.Documents
            .Where(d => d.Id == id)
            .ExecuteUpdateAsync(d => d
                .SetProperty(doc => doc.Status, ProcessingStatus.Processing)
                .SetProperty(doc => doc.ProcessedAt, DateTime.UtcNow)
                .SetProperty(doc => doc.Text, text), cancellationToken: token);
    }
    
    public async Task SetDocumentStatus(Guid id, ProcessingStatus status)
    {
        await dbContext.Documents
            .Where(d => d.Id == id)
            .ExecuteUpdateAsync(d => d
                .SetProperty(doc => doc.Status, status));
    }
    
    public async Task SetDocumentError(Guid id, string error, CancellationToken token)
    {
        await dbContext.Documents
            .Where(d => d.Id == id)
            .ExecuteUpdateAsync(d => d
                .SetProperty(doc => doc.Status, ProcessingStatus.Error)
                .SetProperty(doc => doc.ErrorMessage, error)
                .SetProperty(doc => doc.ProcessedAt, DateTime.UtcNow), cancellationToken: token);
    }
}