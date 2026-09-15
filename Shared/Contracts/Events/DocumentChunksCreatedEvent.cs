using DocumentService.Dtos;

namespace Shared.Contracts.Events;

public record DocumentChunksCreatedEvent(
    Guid SourceId,
    SourceType SourceType,
    IReadOnlyList<DocumentChunkData> Chunks
);