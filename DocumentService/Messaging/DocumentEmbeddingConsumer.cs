using System.Text;
using System.Text.Json;
using DocumentService.Dtos;
using DocumentService.Repositories;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts.Events;
using Shared.Messaging;

namespace DocumentService.Messaging;

public class DocumentEmbeddingConsumer(
    IOptions<RabbitMqOptions> options, 
    RabbitMqConnectionProvider connectionProvider,
    RabbitMqConsumerInitializer consumerInitializer,
    IServiceScopeFactory serviceScopeFactory) : BackgroundService
{
    private readonly RabbitMqOptions _options = options.Value;
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await connectionProvider.GetChannelAsync();

        await consumerInitializer.DeclareParametersAsync(channel, _options, stoppingToken);
        
        var consumer = new AsyncEventingBasicConsumer(channel);
        consumer.ReceivedAsync += HandleEmbeddingResultAsync;

        await channel.BasicConsumeAsync(
            queue:  _options.QueueName,
            autoAck: false,
            consumer: consumer, 
            cancellationToken: stoppingToken);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task HandleEmbeddingResultAsync(object sender, BasicDeliverEventArgs eventArgs)
    {
        var json = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

        var scope = serviceScopeFactory.CreateScope();
        var documentService = scope.ServiceProvider.GetRequiredService<DocumentRepository>();
        
        var channel = await connectionProvider.GetChannelAsync();
        
        switch (eventArgs.RoutingKey)
        {
            case "embedding.completed":
            {
                var message = JsonSerializer.Deserialize<EmbeddingCompletedEvent>(json);
                
                if (message == null)
                    throw new InvalidOperationException(
                        "Failed to deserialize EmbeddingCompletedEvent.");
                
                if(message.SourceType != SourceType.Document)
                {
                    await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
                    return;
                };
                
                await documentService.SetDocumentStatus(message.Id, ProcessingStatus.Complete);
                
                await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
                break;
            }
            case "embedding.failed":
            {
                var message = JsonSerializer.Deserialize<EmbeddingFailedEvent>(json);
                
                if (message == null)
                    throw new InvalidOperationException(
                        "Failed to deserialize EmbeddingCompletedEvent.");
                
                if(message.SourceType != SourceType.Document)
                {
                    await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
                    return;
                }
                
                await documentService.SetDocumentStatus(message.Id, ProcessingStatus.Error);
                
                await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
                break;
            }
            default:
                throw new InvalidOperationException(
                    $"Unknown routing key: {eventArgs.RoutingKey}");
                
        }
    }
}