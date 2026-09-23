using AssistantService.Abstractions;
using AssistantService.Grpc;
using AssistantService.Services;
using Microsoft.AspNetCore.Server.Kestrel.Core;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IAiService, GeminiService>();

builder.Services.AddGrpc();


builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(5215, listenOptions =>
    {
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});

// Связь с Embedding сервисом
builder.Services.AddGrpcClient<EmbeddingService.Grpc.Embedding.EmbeddingClient>(options =>
{
    options.Address = new Uri("http://localhost:5025");
});

// Связь с Article сервисом
builder.Services.AddGrpcClient<ArticleService.Grpc.Article.ArticleClient>(options =>
{
    options.Address = new Uri("http://localhost:5010");
});

// Связь с Document сервисом
builder.Services.AddGrpcClient<DocumentService.Grpc.Document.DocumentClient>(options =>
{
    options.Address = new Uri("http://localhost:5233");
});

builder.Services.AddScoped<EmbeddingGrpcClient>();
builder.Services.AddScoped<TextDataGrpcClient>();

builder.Services.AddScoped<AssistantProcessor>();

var app = builder.Build();

app.MapGrpcService<AssistantGrpcEndpoint>();

app.Run();

