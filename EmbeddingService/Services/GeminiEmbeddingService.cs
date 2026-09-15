using System.Text.Json.Serialization;
using EmbeddingService.Abstractions;
using Pgvector;

namespace EmbeddingService.Services;

public class GeminiEmbeddingService(HttpClient httpClient) : IEmbeddingService
{
    public async Task<Vector> CreateEmbedding(string? text, CancellationToken cancellationToken = default)
    {
        var request = new
        {
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
        };
        
        var geminiModel = "gemini-embedding-2";
        
        using var response = await httpClient.PostAsJsonAsync(
            $"https://generativelanguage.googleapis.com/v1beta/models/{geminiModel}:embedContent",
            request,
            cancellationToken);
        
        response.EnsureSuccessStatusCode();
        
        var result = await response.Content
            .ReadFromJsonAsync<GeminiEmbeddingResponse>(cancellationToken);
        
        return new Vector(result!.Embedding.Values);
        // var json = await response.Content.ReadAsStringAsync(cancellationToken);
        //
        // Console.WriteLine(json);
        //
        // throw new Exception("Посмотри JSON в консоли");
    }
    

    
    private sealed class GeminiEmbeddingResponse
    {
        [JsonPropertyName("embedding")]
        public EmbeddingResult Embedding { get; set; } = null!;
    }

    private sealed class EmbeddingResult
    {
        [JsonPropertyName("values")]
        public float[] Values { get; set; } = [];
    }
    
}