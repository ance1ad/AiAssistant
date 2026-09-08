using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

namespace DocumentService.Messaging;

public class RabbitMqPublisher
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;
    
    public RabbitMqPublisher()
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

    public async Task PublishAsync<T>(T message)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await _channel.BasicPublishAsync(
            exchange: "document-events",
            routingKey: "document.chunks.created",
            body: body);
    }
}