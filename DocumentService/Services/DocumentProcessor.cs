using DocumentService.Abstractions;
using DocumentService.Messaging;
using DocumentService.Models;
using DocumentService.Repositories;
using Shared.Contracts.Events;

namespace DocumentService.Services;

public class DocumentProcessor(
    DocumentRepository documentRepository, 
    DocumentParserResolver documentParserResolver,
    ITextChunker textChunker, 
    ChunkingService chunkingService,
    RabbitMqPublisher publisher)
{
    public async Task ProcessAsync(
        KnowledgeDocument knowledgeDocument, 
        CancellationToken cancellationToken)
    {
        try
        {
            var documentParser = documentParserResolver
                .Resolve(knowledgeDocument.ContentType);
            
            var text = await documentParser
                .ExtractTextAsync(knowledgeDocument, cancellationToken);
            
            var parsingText = textChunker.Parse(text);

            await chunkingService
                .AddChunksRange(parsingText, knowledgeDocument, cancellationToken);
            
            await documentRepository
                .SetDocumentComplete(knowledgeDocument.Id, text, cancellationToken);
            
            // Уведомить о готовности чанков
            await publisher.PublishAsync(new DocumentChunksCreatedEvent(knowledgeDocument.Id));
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception e)
        {
            await documentRepository
                .SetDocumentError(knowledgeDocument.Id, e.Message, cancellationToken);
            throw;
        }
    }

    public async Task<KnowledgeDocument?> GetPendingDocument(CancellationToken token)
    {
        var document = await documentRepository.GetPendingDocument(token);
        if (document == null)
        {
            Console.WriteLine("Не найдено новых файлов для обработки");
            return null;
        }
        // Принимаем в работу
        Console.WriteLine($"Файл {document.FilePath} принят в работу");
        await documentRepository.SetDocumentProcessing(document.Id, token);
        return document;
    }
}