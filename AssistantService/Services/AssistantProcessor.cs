using AssistantService.Abstractions;
using AssistantService.Grpc;
using EmbeddingService.Grpc;

namespace AssistantService.Services;

public class AssistantProcessor(
    EmbeddingGrpcClient embeddingGrpcClient,
    TextDataGrpcClient textDataGrpcClient,
    ILogger<AssistantProcessor> logger,
    IAiService service)
{
    public async Task<string> AskAsync(string question)
    {
        // Получить совпадения
        SimilaritySearchResults? similarities = await embeddingGrpcClient.GetSimilaritiesAsync(question);

        var knowledgeBase = new List<string>();
        if (similarities != null)
        {
            logger.LogInformation($"Get {similarities.CalculateSize()} similarities");

            Console.WriteLine("Получен текст:");
            for (var i = 0; i < similarities.Results.Count; i++)
            {
                var similarity = similarities.Results[i];
                var type = (Shared.Contracts.Events.SourceType)similarity.SourceType;

                var textAsync = await textDataGrpcClient.GetTextAsync(type, similarity.Id);
                Console.WriteLine($"{i+1}. {textAsync} - счет :{similarity.SimilarityScore} \n\n");
                knowledgeBase.Add($"{i+1}. {textAsync}");
            }
        }
        else
        {
            logger.LogError("Get 0 similarities");
        }

        // Задать вопросик в Gemini
        var result = await service.GenerateAnswer(question, knowledgeBase);

        logger.LogInformation(
            "For question {Question} service generate answer {Answer}", 
            question, 
            result);
        
        return result;
    }
}