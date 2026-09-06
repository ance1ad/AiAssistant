using DocumentService.Abstractions;
using DocumentService.Models;

namespace DocumentService.Services;

public class TxtDocumentParser : IDocumentParser
{
    public bool CanParse(string contentType)
        => contentType == "text/plain";
    
    public async Task<string> ExtractTextAsync(KnowledgeDocument knowledgeDocument, CancellationToken cancellationToken)
    {
        return await File.ReadAllTextAsync(
            knowledgeDocument.FilePath,
            cancellationToken
        );
    }
}