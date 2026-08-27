using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.DocumentParsing;

public class TxtDocumentParser : IDocumentParser
{
    public async Task<string> ExtractTextAsync(KnowledgeDocument knowledgeDocument, CancellationToken cancellationToken)
    {
        return await File.ReadAllTextAsync(
            knowledgeDocument.FilePath,
            cancellationToken
        );
    }
}