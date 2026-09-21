namespace Shared.Contracts.Events;

public record EmbeddingFailedEvent(
    Guid Id,
    SourceType SourceType
) : IEmbeddingResult;