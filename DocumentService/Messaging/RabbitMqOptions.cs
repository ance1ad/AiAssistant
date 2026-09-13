namespace DocumentService.Messaging;

public class RabbitMqOptions
{
    public string HostName { get; set; } = string.Empty;
    public int Port { get; set; }
    public string ExchangeName { get; set; } = string.Empty;
    public string RoutingKey { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}