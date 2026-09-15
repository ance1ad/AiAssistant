using System.Text;
using System.Text.Json;
using EmbeddingService.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts.Events;
using Shared.Messaging;

namespace EmbeddingService.Messaging;

public class RabbitMqConsumer(
    IOptions<RabbitMqOptions> options, 
    RabbitMqConnectionProvider connectionProvider,
    IServiceScopeFactory serviceScopeFactory
    )  : BackgroundService
{
    private readonly RabbitMqOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await connectionProvider.GetChannelAsync();
        
        await channel.QueueDeclareAsync(
            queue: _options.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false, 
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += HandleMessageAsync;

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

        var message = JsonSerializer.Deserialize<DocumentChunksCreatedEvent>(json);

        var channel = await connectionProvider.GetChannelAsync();
        
        Console.WriteLine("Получено событие: {0}, количество чанков {1}", message?.SourceId, message?.Chunks.Count);

        var scope = serviceScopeFactory.CreateScope();
        var embeddingCreator = scope.ServiceProvider.GetRequiredService<EmbeddingCreator>();
        
        if (message != null) 
            await embeddingCreator.CreateVector(message);

        await channel.BasicAckAsync(
            deliveryTag: eventArgs.DeliveryTag, 
            multiple: false);
    }
    
}