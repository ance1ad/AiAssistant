using DocumentService.Services;

namespace DocumentService.Background;

public class DocumentProcessingWorker(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int secondsToCheck = 15;
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine($"Worker running at: {DateTime.Now}");
            
            using var scope = scopeFactory.CreateScope();
            var documentProcessor = scope.ServiceProvider
                .GetRequiredService<DocumentProcessor>();
            
            
            // Найти документ ...
            var doc = await documentProcessor.GetPendingDocument(stoppingToken);
            
            if (doc != null)
            {
                await documentProcessor.ProcessAsync(doc, stoppingToken);
            }
            
            await Task.Delay(
                TimeSpan.FromSeconds(secondsToCheck),
                stoppingToken
            );
        }
    }
}