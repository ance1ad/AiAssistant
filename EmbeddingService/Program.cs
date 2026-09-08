using Shared.Messaging;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHostedService<RabbitMqConsumer>();

var app = builder.Build();

app.MapControllers();

app.Run();
