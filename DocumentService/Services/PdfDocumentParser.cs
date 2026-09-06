using DocumentService.Abstractions;
using DocumentService.Models;

namespace DocumentService.Services;

public class PdfDocumentParser : IDocumentParser
{
    public bool CanParse(string contentType)
        => contentType == "application/pdf";

    public Task<string> ExtractTextAsync(KnowledgeDocument knowledgeDocument, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}