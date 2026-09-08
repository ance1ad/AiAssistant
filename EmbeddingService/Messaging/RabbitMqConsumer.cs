using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts.Events;

namespace Shared.Messaging;

public class RabbitMqConsumer : BackgroundService
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    
    
    public RabbitMqConsumer()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            UserName = "guest",
            Password = "guest"
        };
        
        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _channel.QueueDeclareAsync(
            queue: "embedding-service",
            durable: true,
            exclusive: false,
            autoDelete: false, 
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (_, eventArgs) =>
        {
            var json = Encoding.UTF8.GetString(eventArgs.Body.ToArray());

            var message = JsonSerializer.Deserialize<DocumentChunksCreatedEvent>(json);

            Console.WriteLine("Получено событие: {0}", message?.DocumentId);
            
            await _channel.BasicAckAsync(
                deliveryTag: eventArgs.DeliveryTag, 
                multiple: false, cancellationToken: stoppingToken);
        };

        await _channel.BasicConsumeAsync(
            queue: "embedding-service",
            autoAck: false,
            consumer: consumer, cancellationToken: stoppingToken);
        
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }
}