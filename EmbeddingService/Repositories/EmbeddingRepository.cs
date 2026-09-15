using EmbeddingService.Application;
using EmbeddingService.Models;

namespace EmbeddingService.Repositories;

public class EmbeddingRepository(EmbeddingDbContext context)
{
    public async Task CreateEmbeddings(List<Embedding> embeddingRecords)
    {
        Console.WriteLine("Creating Vector");
    }
    
    // public async Task
}