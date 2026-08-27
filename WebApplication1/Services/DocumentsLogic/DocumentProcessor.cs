using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services.Document;

public class DocumentProcessor(DocumentRepository repository)
{
    public async Task ProcessAsync(KnowledgeDocument knowledgeDocument, CancellationToken cancellationToken)
    {
        
    }

    public async Task<KnowledgeDocument?> GetPendingDocument()
    {
        var document = await repository.GetPendingDocument();
        if (document == null) return null;
        // Принимаем в работу
        await repository.SetDocProcessing(document.Id);
        return document;
    }
}