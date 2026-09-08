using DocumentService.Messaging;
using Shared.Contracts.Events;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Controllers;


[ApiController]
[Route("api/test-rabbit")]
public class RabbitTestController(RabbitMqPublisher publisher) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Test()
    {
        var message = new DocumentChunksCreatedEvent(Guid.NewGuid());

        await publisher.PublishAsync(message);
        
        return Ok(message);
    }
}