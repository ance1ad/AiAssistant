using Microsoft.EntityFrameworkCore;
using WebApplication1.Application;
using WebApplication1.Repositories;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<AssistentDbContext>(options =>
{
    options.UseNpgsql(configuration.GetConnectionString(nameof(AssistentDbContext)));
});
builder.Services.AddScoped<ArticlesRepository>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
}

app.MapControllers();

app.Run();
