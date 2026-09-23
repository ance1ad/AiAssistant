using DocumentService.Repositories;
using Grpc.Core;

namespace DocumentService.Grpc;

public class DocumentGrpcEndpoint(
    ChunkRepository repository,
    ILogger<DocumentGrpcEndpoint> logger) : Document.DocumentBase
{
    public override async Task<ResourceResponse> GetText(ResourceRequest request, ServerCallContext context)
    {
        var chunk = await repository.Get(new Guid(request.Id));
        if (chunk != null)
        {
            return new ResourceResponse {Text = chunk.Text};
        }
        logger.LogError(
            "Document {Id} not found",
            request.Id);
        
        return new ResourceResponse() {Text = string.Empty};
    }
}