using Microsoft.AspNetCore.Mvc;
using TelegramService.Services;
using WebApplication1.Dtos;

namespace TelegramService.Controllers;

[ApiController]
[Route("users")]
public class UsersController(UserService userService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await userService.Get();
        return Ok(users);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByTelegramId(long id)
    { 
        var user = await userService.Get(id);
        if (user != null)
        {
            return Ok(user);
        }
        return NotFound();
    }
    
    [HttpPost]
    public async Task<IActionResult> PostUser(long telegramId, string name)
    { 
        var createdUser = await userService.Create(telegramId, name);
        
        return CreatedAtAction(
            nameof(GetUserByTelegramId), 
            new {id = createdUser.Id},
            createdUser
        );
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(Guid id, UpdateUserRequest updateUser)
    {
        bool updated = await userService.Update(id, updateUser);
    
        if (updated)
        {
            return NoContent();
        }
        return NotFound();
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        bool result = await userService.Delete(id);
        if (result)
        {
            return NoContent();
        }
        return NotFound();
        
    }
}