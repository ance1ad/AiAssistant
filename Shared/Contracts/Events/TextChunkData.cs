namespace DocumentService.Dtos;

public record TextChunkData(
    Guid Id,
    int Index,
    string Text
);