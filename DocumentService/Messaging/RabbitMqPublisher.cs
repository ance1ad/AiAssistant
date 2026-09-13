using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace DocumentService.Messaging;

public class RabbitMqPublisher
{
    private readonly RabbitMqConnectionProvider _connectionProvider;
    private readonly RabbitMqOptions _options;
    
    public RabbitMqPublisher(IOptions<RabbitMqOptions> options, RabbitMqConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
        _options = options.Value;
    }

    public async Task PublishAsync<T>(T message)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var channel = await _connectionProvider.GetChannelAsync(); 
        
        await channel.BasicPublishAsync(
            exchange: _options.ExchangeName,
            routingKey: _options.RoutingKey,
            body: body);
    }
}