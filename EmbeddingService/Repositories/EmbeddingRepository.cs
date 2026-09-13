using EmbeddingService.Application;

namespace EmbeddingService.Repositories;

public class EmbeddingRepository(EmbeddingDbContext context)
{
    public async Task CreateVector(Guid id, List<float> vectors)
    {
        Console.WriteLine("Creating Vector");
    }
    
    // public async Task
}