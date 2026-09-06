namespace DocumentService.Abstractions;

public interface ITextChunker
{
    public IReadOnlyList<string> Parse(string text);
}