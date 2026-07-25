using Microsoft.EntityFrameworkCore;
using WebApplication1.Application;
using WebApplication1.Repositories;
using WebApplication1.Services;
using WebApplication1.Telegram;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddDbContext<AssistentDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString(nameof(AssistentDbContext)));
});

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddScoped<ArticleService>();
builder.Services.AddScoped<ArticlesRepository>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<UsersRepository>();

builder.Services.AddSingleton<TelegramBotService>();
builder.Services.AddSingleton<TelegramUpdateHandler>();


var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
}

app.MapControllers();

var bot =  app.Services.GetRequiredService<TelegramBotService>();
bot.Start();


app.Run();
