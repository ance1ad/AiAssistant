using DocumentService.Dtos;
using DocumentService.Models;
using DocumentService.Repositories;
using Shared.Contracts.Events;

namespace DocumentService.Services;

public class DocumentService(DocumentRepository repository)
{
    public async Task<DocumentResponse> Upload(IFormFile file)
    {
        var docId = Guid.NewGuid();
        var extension = Path.GetExtension(file.FileName);
        var fileName = $"{docId}{extension}";

        var storagePath = Path.Combine(
            "Storage", "Documents", fileName   
        );
        
        Directory.CreateDirectory(Path.GetDirectoryName(storagePath)!);
        
        await using var stream = new FileStream(storagePath, FileMode.Create);
        
        await file.CopyToAsync(stream);
        
        var document = new KnowledgeDocument()
        {
            Id = docId,
            FileName = file.FileName,
            FilePath = storagePath,
            ContentType = file.ContentType,
            Status = ProcessingStatus.Pending,
            CreatedAt = DateTime.UtcNow,
        };

        await repository.Add(document);
        
        return new DocumentResponse(
            document.Id,
            document.FileName,
            document.Status);
    }
}