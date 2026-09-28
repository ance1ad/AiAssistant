using System.Text;
using System.Text.Json;
using EmbeddingService.Services;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts.Events;
using Shared.Messaging;
using Shared.Messaging.Configuration;

namespace EmbeddingService.Messaging;

public class EmbeddingConsumer(
    IOptions<RabbitMqOptions> options, 
    RabbitMqConnectionProvider connectionProvider,
    RabbitMqPublisher publisher,
    RabbitMqConsumerInitializer initializer,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<EmbeddingConsumer> logger)  : BackgroundService
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
        var channel = await connectionProvider.GetChannelAsync();

        var message = Deserialize(eventArgs);

        if (message == null)
        {
            logger.LogError("Message is null");
            
            await Ack(eventArgs, channel);
            
            return;
        }
        try
        {
            await ProcessMessage(message);
            
            await publisher.PublishAsync(
                new EmbeddingCompletedEvent(
                    message.SourceId,
                    message.SourceType), 
                RabbitEvents.EmbeddingCompleted);
            
            await Ack(eventArgs, channel);
        }
        catch(Exception ex)
        {
            logger.LogError(ex, "Failed to process embedding");
            
            await publisher.PublishAsync(
                new EmbeddingFailedEvent(
                    message.SourceId,
                    message.SourceType), 
                RabbitEvents.EmbeddingFailed);
            
            await Ack(eventArgs, channel);
        }
    }

    private static TextChunksPreparedEvent? Deserialize(BasicDeliverEventArgs eventArgs)
    {
        var json = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
        var message = JsonSerializer.Deserialize<TextChunksPreparedEvent>(json);
        return message;
    }

    private static async Task Ack(BasicDeliverEventArgs eventArgs, IChannel channel)
    {
        await channel.BasicAckAsync(
            deliveryTag: eventArgs.DeliveryTag, 
            multiple: false);
    }
    
    private async Task ProcessMessage(TextChunksPreparedEvent message)
    {
        using var scope = serviceScopeFactory.CreateScope();

        var embeddingCreator =
            scope.ServiceProvider.GetRequiredService<EmbeddingProcessor>();

        await embeddingCreator.AddVectors(message);
    }
}