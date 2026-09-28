namespace Shared.Contracts.Models;

public record TextChunkData(
    Guid Id,
    int Index,
    string Text
);