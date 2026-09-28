using AssistantService.Services;
using Grpc.Core;

namespace AssistantService.Grpc;

public class AssistantGrpcEndpoint(
    AssistantProcessor assistantProcessor,
    ILogger<AssistantGrpcEndpoint> logger) : Assistant.AssistantBase
{
    public override async Task<AskResponse> Ask(AskRequest request, ServerCallContext context)
    {
        try
        {
            var answer = await assistantProcessor.AskAsync(request.Question);
        
            return await Task.FromResult(new AskResponse
            {
                Answer = answer
            });
        }
        catch(Exception e)
        {
            logger.LogError(
                "Error on AssistantProcessor {Error}",
                e.Message);
            
            return await Task.FromResult(new AskResponse
            {
                Answer = string.Empty
            });
        }
        
        
    }
}