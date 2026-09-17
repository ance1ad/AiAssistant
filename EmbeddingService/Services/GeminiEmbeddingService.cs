using System.Text.Json.Serialization;
using EmbeddingService.Abstractions;
using Pgvector;

namespace EmbeddingService.Services;

public class GeminiEmbeddingService(HttpClient httpClient) : IEmbeddingService
{
    public async Task<IReadOnlyList<Vector>> CreateEmbeddings(
        IReadOnlyList<string> texts, 
        CancellationToken cancellationToken = default)
    {
        var geminiModel = "gemini-embedding-2";
        
        var request = new
        {
            requests = texts.Select(text => new
            {
                model = $"models/{geminiModel}",
                content = new
                {
                    parts = new[]
                    {
                        new
                        {
                            text
                        }
                    }
                },
                output_dimensionality = 768
            }).ToList()
        };
        
        
        using var response = await httpClient.PostAsJsonAsync(
            $"https://generativelanguage.googleapis.com/v1beta/models/{geminiModel}:batchEmbedContents",
            request,
            cancellationToken);
        
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content
            .ReadFromJsonAsync<GeminiEmbeddingResponse>(cancellationToken);
        
        return result!.Embeddings
            .Select(x => new Vector(x.Values))
            .ToList();
    }
    
    
    private sealed class GeminiEmbeddingResponse
    {
        [JsonPropertyName("embeddings")]
        public EmbeddingResult[] Embeddings { get; set; } = null!;
    }

    private sealed class EmbeddingResult
    {
        [JsonPropertyName("values")]
        public float[] Values { get; set; } = [];
    }

}