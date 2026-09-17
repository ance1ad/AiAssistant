using DocumentService.Dtos;

namespace Shared.Contracts.Events;

public record TextChunksPreparedEvent(
    Guid SourceId,
    SourceType SourceType,
    IReadOnlyList<TextChunkData> Chunks
);