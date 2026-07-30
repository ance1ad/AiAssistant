using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Controllers;


[ApiController]
[Route("auth")]
public class AuthController(AdminService service) : ControllerBase
{
    private readonly AdminService _service = service;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginAdminDto loginAdminDto)
    {
        var token = await _service.Login(loginAdminDto);
        if (token == null)
            return Unauthorized();
        
        return Ok(new { token });
    } 
    
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterAdminDto loginAdminDto)
    {
        var adminDto = await _service.Register(loginAdminDto);
        return Ok(adminDto);    
    } 
}