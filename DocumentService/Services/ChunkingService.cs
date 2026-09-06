using DocumentService.Models;
using DocumentService.Repositories;

namespace DocumentService.Services;

public class ChunkingService(ChunkRepository repository)
{
    public async Task AddChunksRange(IReadOnlyList<string> splitedText, KnowledgeDocument document, CancellationToken token)
    {
        var chunksEntity = new List<DocumentChunk>();
        for (var index = 0; index < splitedText.Count; index++)
        {
            chunksEntity.Add(new DocumentChunk
            {
                Id = Guid.NewGuid(),
                KnowledgeDocument = document,
                KnowledgeDocumentId = document.Id,
                ChunkIndex = index,
                Text = splitedText[index]
            });
        }
        await repository.AddChunksRange(chunksEntity, token);
    }
}