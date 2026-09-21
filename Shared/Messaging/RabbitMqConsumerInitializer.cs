using RabbitMQ.Client;

namespace Shared.Messaging;

public class RabbitMqConsumerInitializer
{
    public async Task DeclareParametersAsync(
        IChannel channel, 
        RabbitMqOptions options, 
        CancellationToken stoppingToken)
    {
        await channel.ExchangeDeclareAsync(
            exchange: options.ExchangeName,
            type: ExchangeType.Direct,
            durable: true,
            autoDelete: false,
            cancellationToken: stoppingToken);
        
        await channel.QueueDeclareAsync(
            queue: options.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false, 
            cancellationToken: stoppingToken);
        
        foreach (var bindingKey in options.BindingKeys)
        {
            await channel.QueueBindAsync(
                queue: options.QueueName,
                exchange: options.ExchangeName,
                routingKey: bindingKey,
                cancellationToken: stoppingToken);
        }
        
        await channel.BasicQosAsync(
            prefetchSize: 0,
            prefetchCount: 1,
            global: false, 
            cancellationToken: stoppingToken);
    }
}