using ArticleService.Grpc;
using Grpc.Core;
using WebApplication1.Repositories;

namespace WebApplication1.Grpc;

public class ArticleGrpcEndpoint(
    ArticlesRepository repository,
    ILogger<ArticleGrpcEndpoint> logger) : Article.ArticleBase
{
    public override async Task<ResourceResponse> GetText(ResourceRequest request, ServerCallContext context)
    {
        var article = await repository.Get(new Guid(request.Id));
        if (article != null)
        {
            return new ResourceResponse {Text = article.Content};
        }
        logger.LogError(
            "Article {Id} not found",
            request.Id);
        
        return new ResourceResponse() {Text = string.Empty};
    }
}