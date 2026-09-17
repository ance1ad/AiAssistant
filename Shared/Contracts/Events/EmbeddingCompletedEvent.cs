using Shared.Contracts.Events;

namespace DocumentService.Dtos;

public record EmbeddingCompletedEvent(
    Guid Id,
    SourceType SourceType
);