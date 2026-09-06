using DocumentService.Models;

namespace DocumentService.Abstractions;

public interface IDocumentParser
{
    bool CanParse(string contentType);
    
    Task<string> ExtractTextAsync(
        KnowledgeDocument knowledgeDocument, 
        CancellationToken cancellationToken
    );
}