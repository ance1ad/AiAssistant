using ArticleService.Grpc;
using DocumentService.Grpc;
using Shared.Contracts.Events;

namespace AssistantService.Grpc;

public class TextDataGrpcClient(
    Document.DocumentClient documentClient,
    Article.ArticleClient articleClient,
    ILogger<TextDataGrpcClient> logger)
{
    public async Task<string> GetTextAsync(SourceType sourceType, string resourceId)
    {
        string resultString = string.Empty;
        switch (sourceType)
        {
            case SourceType.Article:
            {
                var result = await articleClient.GetTextAsync(
                    new ArticleService.Grpc.ResourceRequest() {Id = resourceId});
                resultString = result.Text;
                break;
            }
            case SourceType.Document:
            {
                var result =  await documentClient.GetTextAsync(
                    new DocumentService.Grpc.ResourceRequest() {Id = resourceId});
                resultString = result.Text;
                break;
            }
        }

        if (resultString == string.Empty)
        {
            logger.LogError($"{nameof(GetTextAsync)} returned an empty string.");
        }
        return resultString;
    }
}