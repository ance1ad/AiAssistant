using EmbeddingService.Grpc;

namespace AssistantService.Grpc;

/// <summary>
/// Обращается за similarities к Embedding микрорсервису
/// </summary>
/// <param name="embeddingClient"></param>
public class EmbeddingGrpcClient(Embedding.EmbeddingClient embeddingClient)
{
    public async Task<SimilaritySearchResults?> GetSimilaritiesAsync(string question)
    {
        return await embeddingClient.GetSimilaritiesAsync(new EmbeddingService.Grpc.AskRequest
        {
            Question = question
        });
    }
}