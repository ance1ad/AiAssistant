using Shared.Contracts.Events;

namespace DocumentService.Dtos;

public record EmbeddingFailedEvent(
    Guid Id,
    SourceType SourceType
);