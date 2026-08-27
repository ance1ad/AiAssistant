using WebApplication1.Models;

namespace WebApplication1.Interfaces;

public interface IDocumentParser
{
    Task<string> ExtractTextAsync(
        KnowledgeDocument knowledgeDocument, 
        CancellationToken cancellationToken
    );
}