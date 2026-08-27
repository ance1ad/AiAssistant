using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("documents")]
public class DocumentsController(DocumentService documentService) : ControllerBase
{
    private static readonly HashSet<string> AllowedContentTypes =
    [
        "application/pdf",
        "text/plain"
    ];
    
    
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile? file)
    {
        const long maxFileSize = 10 * 1024 * 1024;

        if(file is null || file.Length == 0)
            return BadRequest("File is empty");
        
        if(file.Length > maxFileSize)
            return BadRequest("File is too large");
        
        if (!AllowedContentTypes.Contains(file.ContentType))
            return BadRequest("This file type is not supported");
        

        var doc = await documentService.Upload(file);
        
        return Accepted(doc); 
    }
}