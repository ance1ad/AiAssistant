using EmbeddingService.Repositories;

namespace EmbeddingService.Services;

public class EmbeddingCreator(EmbeddingRepository repository)
{
    public async Task CreateVector(Guid id)
    {
        var vectors = new List<float>();
        
        await repository.CreateVector(id, vectors);
    }
}