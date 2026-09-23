namespace Shared.Contracts.Events;

public record SimilarityResult
(
    Guid Id,
    SourceType SourceType,
    double SimilarityScore
);