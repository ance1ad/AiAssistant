using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.DocumentParsing;

public class PdfDocumentParser : IDocumentParser
{
    public Task<string> ExtractTextAsync(KnowledgeDocument knowledgeDocument, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}