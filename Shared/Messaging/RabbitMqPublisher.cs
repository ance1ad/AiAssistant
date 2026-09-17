using System.Text;
using System.Text.Json;
using DocumentService.Messaging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Shared.Messaging;

public class RabbitMqPublisher(
    IOptions<RabbitMqOptions> options, 
    RabbitMqConnectionProvider connectionProvider)
{
    private readonly RabbitMqOptions _options = options.Value;

    public async Task PublishAsync<T>(T message, string routingKey)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var channel = await connectionProvider.GetChannelAsync(); 
        
        await channel.BasicPublishAsync(
            exchange: _options.ExchangeName,
            routingKey: routingKey,
            body: body);
    }
}