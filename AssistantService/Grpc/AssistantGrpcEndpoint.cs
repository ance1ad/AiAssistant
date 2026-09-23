using AssistantService.Services;
using Grpc.Core;

namespace AssistantService.Grpc;

public class AssistantGrpcEndpoint(AssistantProcessor assistantProcessor) : Assistant.AssistantBase
{
    public override async Task<AskResponse> Ask(AskRequest request, ServerCallContext context)
    {
        var answer2 = $"Я получил твой вопрос {request.Question}";

        var answer = await assistantProcessor.AskAsync(request.Question);
        
        return await Task.FromResult(new AskResponse
        {
            Answer = answer
        });
    }
}