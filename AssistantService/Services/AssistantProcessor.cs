using AssistantService.Abstractions;
using AssistantService.Grpc;
using EmbeddingService.Grpc;
using SourceType = Shared.Contracts.Models.SourceType;

namespace AssistantService.Services;

public class AssistantProcessor(
    EmbeddingGrpcClient embeddingGrpcClient,
    TextDataGrpcClient textDataGrpcClient,
    ILogger<AssistantProcessor> logger,
    IAiService service)
{
    private float SimilarityThreshold { get; } = 0.7f;

    public async Task<string> AskAsync(string question)
    {
        // Получить совпадения
        SimilaritySearchResults? similarities = await embeddingGrpcClient.GetSimilaritiesAsync(question);

        var knowledgeBase = new List<string>();
        if (similarities != null)
        {
            logger.LogInformation(
                "Get {Count} similarities",
                similarities.Results.Count);

            Console.WriteLine("Получен текст:");
            for (var i = 0; i < similarities.Results.Count; i++)
            {
                var similarity = similarities.Results[i];
                
                // Минимально допустимое 
                if(similarity.SimilarityScore < SimilarityThreshold) continue;
                
                var type = (SourceType)similarity.SourceType;
                // similarity.SimilarityScore
                var textAsync = await textDataGrpcClient.GetTextAsync(type, similarity.Id);
                knowledgeBase.Add($"{i+1}. {textAsync}");
            }

            CheckSimilarityData(similarities, knowledgeBase);
        }
        else
        {
            logger.LogError("Get 0 similarities");
        }

        if (knowledgeBase.Count == 0)
        {
            logger.LogError($"KnowledgeBase is empty for question \"{question}\"");
            return string.Empty;
        }
        
        // Задать вопросик в Gemini
        var result = await service.GenerateAnswer(question, knowledgeBase);

        logger.LogInformation(
            "For question {Question} service generate answer {Answer}", 
            question, 
            result);
        
        return result;
    }

    private static void CheckSimilarityData(SimilaritySearchResults similarities, List<string> knowledgeBase)
    {
        Console.WriteLine("Similarities:");
        for (var i = 0; i < similarities.Results.Count; i++)
        {
            var similarity = similarities.Results[i];
            Console.WriteLine(
                $"{i + 1}. " +
                $"Type: {similarity.SourceType}, " +
                $"Id: {similarity.Id}, " +
                $"Score: {similarity.SimilarityScore:F6}");
        }
        Console.WriteLine("Final data for answer:");
        for (var i = 0; i < knowledgeBase.Count; i++)
        {
            Console.WriteLine($"{i+1}. {knowledgeBase[i]}");
        }
        
    }
}