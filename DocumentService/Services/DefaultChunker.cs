using DocumentService.Abstractions;

namespace DocumentService.Services;

public class DefaultChunker : ITextChunker
{
    public IReadOnlyList<string> Parse(string text)
    {
        int chunkSize = 100;
        int overlap = 15;
        return ChunkParse(text, chunkSize, overlap);
    }
    
    private IReadOnlyList<string> ChunkParse(string text, int chunkSize, int overlap)
    {
        if (chunkSize <= 0) 
            throw new ArgumentOutOfRangeException(nameof(chunkSize));
        
        if (overlap < 0 || overlap >= chunkSize) 
            throw new ArgumentOutOfRangeException(nameof(overlap));
        
        string[] words = text.Split(
            [' ','\r','\n','\t'],
            StringSplitOptions.RemoveEmptyEntries
        );
        
        List<string> chunks = new();
        
        for (int i = 0; i < words.Length; i += chunkSize - overlap)
        {
            int slicedSize = Math.Min(chunkSize, words.Length - i);
            
            string chunk = string.Join(" ", words, i, slicedSize);
            
            chunks.Add(chunk);
        }
        return chunks;
    }
}