using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("users")]
public class UsersController(UserService userService) : ControllerBase
{
    private readonly UserService _userService = userService;
    
    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _userService.Get();
        return Ok(users);
    }
    
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserByTelegramId(long id)
    { 
        var user = await _userService.Get(id);
        if (user != null)
        {
            return Ok(user);
        }
        return NotFound();
    }
    
    
    [HttpPost]
    public async Task<IActionResult> PostUser(long telegramId, string name)
    { 
        var createdUser = await _userService.Create(telegramId, name);
        
        return CreatedAtAction(
            nameof(GetUserByTelegramId), 
            new {id = createdUser.Id},
            createdUser
        );
    }

    
    [HttpPut("{id}")]
    public async Task<IActionResult> PutUser(Guid id, UpdateUserDto updateUser)
    {
        bool updated = await _userService.Update(id, updateUser);
    
        if (updated)
        {
            return NoContent();
        }
        return NotFound();
    }
    
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        bool result = await _userService.Delete(id);
        if (result)
        {
            return NoContent();
        }
        return NotFound();
        
    }
}