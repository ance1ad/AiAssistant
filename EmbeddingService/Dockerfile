FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore "EmbeddingService/EmbeddingService.csproj"
RUN dotnet publish "EmbeddingService/EmbeddingService.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore
    
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5025

ENTRYPOINT ["dotnet", "EmbeddingService.dll"]