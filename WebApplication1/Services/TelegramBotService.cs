using Telegram.Bot;
using WebApplication1.Telegram;

namespace WebApplication1.Services;

public class TelegramBotService
{
    private readonly TelegramBotClient _client;
    private readonly TelegramUpdateHandler _updateHandler;
    
    
    public TelegramBotService(IConfiguration configuration, 
        TelegramUpdateHandler updateHandler)
    {
        var token = configuration["Telegram:BotToken"];
        _client = new TelegramBotClient(token);

        _updateHandler = updateHandler;
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
        Console.WriteLine(exception.Message);

        return Task.CompletedTask;
    }
}