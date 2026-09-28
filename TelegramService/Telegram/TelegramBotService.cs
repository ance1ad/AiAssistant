using Telegram.Bot;

namespace TelegramService.Telegram;

public class TelegramBotService
{
    private readonly TelegramBotClient _client;
    private readonly TelegramUpdateHandler _updateHandler;
    private readonly ILogger<TelegramBotService> _logger;

    public TelegramBotService(
        IConfiguration configuration, 
        TelegramUpdateHandler updateHandler,
        ILogger<TelegramBotService> logger)
    {
        var token = configuration["Telegram:BotToken"]; // возьми из секретов
        _client = new TelegramBotClient(token);

        _updateHandler = updateHandler;
        _logger = logger;
    }

    public void Start()
    {
        _client.StartReceiving(
            _updateHandler.HandleUpdate,
            HandleError
        );
    }
    
    private Task HandleError(
        ITelegramBotClient botClient,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception, 
            exception.Message);

        return Task.CompletedTask;
    }
}