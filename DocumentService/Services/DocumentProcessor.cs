using DocumentService.Abstractions;
using DocumentService.Dtos;
using DocumentService.Models;
using DocumentService.Repositories;
using Shared.Contracts.Events;
using Shared.Messaging;

namespace DocumentService.Services;

public class DocumentProcessor(
    DocumentRepository documentRepository, 
    DocumentParserResolver documentParserResolver,
    ITextChunker textChunker, 
    ChunkingService chunkingService,
    RabbitMqPublisher publisher,
    ILogger<DocumentProcessor> logger)
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

            var chunks = await chunkingService
                .AddChunksRange(parsingText, knowledgeDocument, cancellationToken);

            var chunksData = chunks
                .Select(x => new TextChunkData(
                    x.Id,
                    x.ChunkIndex,
                    x.Text
                )).ToList();
            
            // Передать чанки
            await publisher.PublishAsync(new TextChunksPreparedEvent(
                knowledgeDocument.Id, SourceType.Document, chunksData), "text.chunks.prepared");
            
            logger.LogInformation(
                "Document {FileName} submitted for processing ",
                knowledgeDocument.FilePath);
            
            await documentRepository
                .SetDocumentProcessed(knowledgeDocument.Id, text, cancellationToken);
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
            return null;
        }

        logger.LogInformation(
            "File: {FileName} is on processing now",
            document.FilePath);
        
        await documentRepository.SetDocumentProcessing(document.Id, token);
        return document;
    }
}