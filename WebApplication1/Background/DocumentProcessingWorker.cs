using WebApplication1.Models;
using WebApplication1.Services.Document;

namespace WebApplication1.Background;

public class DocumentProcessingWorker(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int secondsToCheck = 4;
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine($"Worker running at: {DateTime.Now}");
            
            using var scope = scopeFactory.CreateScope();
            var documentProcessor = scope.ServiceProvider
                .GetRequiredService<DocumentProcessor>();
            
            
            // Найти документ ...
            await documentProcessor.ProcessAsync(new KnowledgeDocument(), stoppingToken);
            
            await Task.Delay(1000 * secondsToCheck, stoppingToken);
        }
    }
}