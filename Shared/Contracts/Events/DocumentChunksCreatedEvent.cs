namespace Shared.Contracts.Events;

public record DocumentChunksCreatedEvent(
    Guid DocumentId
);