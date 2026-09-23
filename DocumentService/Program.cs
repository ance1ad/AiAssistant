using DocumentService.Abstractions;
using DocumentService.Application;
using DocumentService.Background;
using DocumentService.Grpc;
using DocumentService.Messaging;
using DocumentService.Repositories;
using DocumentService.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.EntityFrameworkCore;
using Shared.Messaging;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddDbContext<DocumentDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("AssistentDbContext"));
});

builder.Services.AddGrpc();

// Конфигурируем на http2 
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5233, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});


builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<DocumentService.Services.DocumentService>();
builder.Services.AddScoped<DocumentRepository>();

builder.Services.AddScoped<DocumentProcessor>();

builder.Services.AddScoped<DocumentParserResolver>();


// Parsers
builder.Services.AddScoped<IDocumentParser, TxtDocumentParser>();
builder.Services.AddScoped<IDocumentParser, PdfDocumentParser>();

builder.Services.AddScoped<ITextChunker, DefaultChunker>();
builder.Services.AddScoped<ChunkRepository>();
builder.Services.AddScoped<ChunkingService>();

// Broker
builder.Services.AddSingleton<RabbitMqPublisher>();
builder.Services.AddSingleton<RabbitMqConnectionProvider>();
builder.Services.AddSingleton<RabbitMqConsumerInitializer>();

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMq"));

// Background
builder.Services.AddHostedService<DocumentProcessingWorker>();
builder.Services.AddHostedService<DocumentEmbeddingConsumer>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.MapGrpcService<DocumentGrpcEndpoint>();

app.Run();