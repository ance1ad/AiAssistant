using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts.Events;
using Shared.Messaging;

namespace EmbeddingService.Messaging;

public class RabbitMqConsumer : BackgroundService
{
    private readonly RabbitMqConnectionProvider _connectionProvider;
    private readonly RabbitMqOptions _options;
    
    public RabbitMqConsumer(IOptions<RabbitMqOptions> options, RabbitMqConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        _options = options.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var channel = await _connectionProvider.GetChannelAsync();
        
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

        var channel = await _connectionProvider.GetChannelAsync();
        
        Console.WriteLine("Получено событие: {0}", message?.DocumentId);
            
        await channel.BasicAckAsync(
            deliveryTag: eventArgs.DeliveryTag, 
            multiple: false);
    }
    
}