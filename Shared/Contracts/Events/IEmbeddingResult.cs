namespace Shared.Contracts.Events;

public interface IEmbeddingResult
{
    public Guid Id { get; }
    public SourceType SourceType { get; }
}