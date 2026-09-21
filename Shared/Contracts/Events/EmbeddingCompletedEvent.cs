namespace Shared.Contracts.Events;

public record EmbeddingCompletedEvent(
    Guid Id,
    SourceType SourceType
) : IEmbeddingResult;