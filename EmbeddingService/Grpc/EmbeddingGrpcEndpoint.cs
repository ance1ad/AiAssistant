using EmbeddingService.Services;
using Grpc.Core;
using Shared.Contracts.Events;

namespace EmbeddingService.Grpc;

public class EmbeddingGrpcEndpoint(
    EmbeddingProcessor embeddingProcessor) : Embedding.EmbeddingBase
{
    public override async Task<SimilaritySearchResults> GetSimilarities(
        AskRequest request, 
        ServerCallContext context)
    {
        List<SimilarityResult> resultList = await embeddingProcessor.FindSimilarities(request.Question);

        var result = new SimilaritySearchResults();
        
        result.Results.AddRange(
            resultList.Select(x => new SimilaritySearchResult
            {
                Id = x.Id.ToString(),
                SourceType = (SourceType)x.SourceType,
                SimilarityScore = x.SimilarityScore
            })
            .ToList()
        );
        
        return result;
    }
}