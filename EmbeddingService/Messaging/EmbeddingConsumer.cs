using System.Text;
using System.Text.Json;
using DocumentService.Dtos;
using DocumentService.Messaging;
using EmbeddingService.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts.Events;
using Shared.Messaging;

namespace EmbeddingService.Messaging;

public class EmbeddingConsumer(
    IOptions<RabbitMqOptions> options, 
    RabbitMqConnectionProvider connectionProvider,
    RabbitMqPublisher publisher,
    RabbitMqConsumerInitializer initializer,
    IServiceScopeFactory serviceScopeFactory
    )  : BackgroundService
{
    private readonly RabbitMqOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await connectionProvider.GetChannelAsync();

        await initializer.DeclareParametersAsync(channel, _options, stoppingToken);
        
        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += HandleMessageAsync;
        
        // Начать доставлять сообщения этому консюмеру
        await channel.BasicConsumeAsync(
            queue: _options.QueueName,
            autoAck: false,
            consumer: consumer, 
            cancellationToken: stoppingToken);
        
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    private async Task HandleMessageAsync(object sender, BasicDeliverEventArgs eventArgs)
    {
        var json = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

        var message = JsonSerializer.Deserialize<TextChunksPreparedEvent>(json);

        var channel = await connectionProvider.GetChannelAsync();
        
        Console.WriteLine("Получено событие: {0}, количество чанков {1}", message?.SourceId, message?.Chunks.Count);

        if (message == null)
        {
            //...
        }

        try
        {
            var scope = serviceScopeFactory.CreateScope();
            var embeddingCreator = scope.ServiceProvider.GetRequiredService<EmbeddingCreator>();
            await embeddingCreator.CreateVector(message);
            
            await publisher.PublishAsync(
                new EmbeddingCompletedEvent(
                    message.SourceId,
                    message.SourceType), 
                "embedding.completed");
            
            await channel.BasicAckAsync(
                deliveryTag: eventArgs.DeliveryTag, 
                multiple: false);
        }
        catch
        {
            await publisher.PublishAsync(
                new EmbeddingFailedEvent(
                    message.SourceId,
                    message.SourceType), 
                "embedding.failed");
            
            await channel.BasicAckAsync(
                deliveryTag: eventArgs.DeliveryTag, 
                multiple: false);
        }
    }
    
}