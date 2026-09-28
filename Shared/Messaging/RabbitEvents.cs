namespace Shared.Contracts.Events;

public static class RabbitEvents
{
    public const string EmbeddingCompleted = "embedding.completed";
    public const string EmbeddingFailed = "embedding.failed";
    public const string TextChunksPrepared = "text.chunks.prepared";
}