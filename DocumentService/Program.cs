using DocumentService.Abstractions;
using DocumentService.Application;
using DocumentService.Background;
using DocumentService.Repositories;
using DocumentService.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;


builder.Services.AddDbContext<DocumentDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString("AssistentDbContext"));
});

builder.Services.AddControllers();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<DocumentService.Services.DocumentService>();
builder.Services.AddScoped<DocumentRepository>();

builder.Services.AddHostedService<DocumentProcessingWorker>();

builder.Services.AddScoped<DocumentProcessor>();

builder.Services.AddScoped<DocumentParserResolver>();


// Parsers
builder.Services.AddScoped<IDocumentParser, TxtDocumentParser>();
builder.Services.AddScoped<IDocumentParser, PdfDocumentParser>();

builder.Services.AddScoped<ITextChunker, DefaultChunker>();
builder.Services.AddScoped<ChunkRepository>();
builder.Services.AddScoped<ChunkingService>();

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();


app.Run();