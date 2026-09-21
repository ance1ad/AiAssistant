using System.Text;
using System.Text.Json;
using DocumentService.Dtos;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts.Events;
using Shared.Messaging;
using WebApplication1.Services;

namespace WebApplication1.Messaging;

public class ArticleEmbeddingConsumer(
    IOptions<RabbitMqOptions> options, 
    RabbitMqConnectionProvider connectionProvider,
    RabbitMqConsumerInitializer consumerInitializer,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<ArticleEmbeddingConsumer> logger) : BackgroundService
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
        var channel = await connectionProvider.GetChannelAsync();

        try
        {
            var json = Encoding.UTF8.GetString(eventArgs.Body.ToArray());
            
            switch (eventArgs.RoutingKey)
            {
                case "embedding.completed":
                {
                    await HandleArticleEmbeddingMessage<EmbeddingCompletedEvent>(
                        channel, eventArgs, json, ProcessingStatus.Complete);

                    break;
                }
                case "embedding.failed":
                {
                    await HandleArticleEmbeddingMessage<EmbeddingFailedEvent>(
                        channel, eventArgs, json, ProcessingStatus.Error);
                    break;
                }
                default:
                    throw new InvalidOperationException(
                        $"Unknown routing key: {eventArgs.RoutingKey}");
            }
        }
        catch (JsonException ex)
        {
            logger.LogError(
                ex, 
                "Invalid JSON in RabbitMQ message. RoutingKey: {RoutingKey}", 
                eventArgs.RoutingKey);
            
            await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex, 
                "Failed to process RabbitMQ message. RoutingKey: {RoutingKey}", 
                eventArgs.RoutingKey);

            await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
        }
    }

    private async Task HandleArticleEmbeddingMessage<T>(
        IChannel channel,
        BasicDeliverEventArgs eventArgs,
        string json,
        ProcessingStatus status) 
    where T : IEmbeddingResult 
    {
        using var scope = serviceScopeFactory.CreateScope();
        var articleService = scope.ServiceProvider.GetRequiredService<ArticleService>();

        var message = JsonSerializer.Deserialize<T>(json);

        if (message == null)
        {
            logger.LogError(
                "Failed to deserialize {MessageType}, RoutingKey: {RoutingKey}",
                typeof(T).Name,
                eventArgs.RoutingKey);

            throw new InvalidOperationException(
                $"Failed to deserialize {typeof(T).Name}.");
        }

        if (message.SourceType != SourceType.Article)
        {
            await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
            return;
        }
        
        await articleService.SetArticleStatus(message.Id, status);

        logger.LogInformation(
            "Article {Article} is turned to status {Status}", 
            message.Id,
            status);

        await channel.BasicAckAsync(eventArgs.DeliveryTag, false);
    }

}