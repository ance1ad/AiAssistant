using Shared.Contracts.Events;

namespace Shared.Contracts.Models;

public record SimilarityResult
(
    Guid Id,
    SourceType SourceType,
    double SimilarityScore
);