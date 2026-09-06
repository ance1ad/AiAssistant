using DocumentService.Abstractions;

namespace DocumentService.Services;

public class DocumentParserResolver(IEnumerable<IDocumentParser> parsers)
{
    private IEnumerable<IDocumentParser> Parsers { get; } = parsers;

    public IDocumentParser Resolve(string contentType)
    {
        var parser = Parsers.FirstOrDefault(p => p.CanParse(contentType));

        return parser 
               ?? throw new NotSupportedException($"No parser found for {contentType}"); 
    }
}