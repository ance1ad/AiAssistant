namespace DocumentService.Dtos;

public record DocumentChunkData(
    Guid Id,
    int Index,
    string Text
);