using EmbeddingService.Messaging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Shared.Messaging;

public class RabbitMqConnectionProvider
{
    private readonly RabbitMqOptions _options;
    private IConnection? _connection; 
    private IChannel? _channel;
    private readonly ConnectionFactory _factory;
    
    
    public RabbitMqConnectionProvider(IOptions<RabbitMqOptions> options)
    {
        _options = options.Value;
        
        _factory = new ConnectionFactory()
        {
            HostName = _options.HostName,
            Port = _options.Port,
            UserName = _options.UserName,
            Password = _options.Password
        };
    }

    public async Task<IConnection> GetConnectionAsync()
    {
        if (_connection != null && _connection.IsOpen)
        {
            return _connection;
        }
        _connection = await _factory.CreateConnectionAsync();
        return _connection;
    }
    
    public async Task<IChannel> GetChannelAsync()
    {
        var connection = await GetConnectionAsync();
        if (_channel != null)
        {
            return _channel;
        }
        _channel = await connection.CreateChannelAsync();
        return _channel;
    }
}