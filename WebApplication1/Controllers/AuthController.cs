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
    public async Task<IActionResult> Login(LoginAdminRequest loginAdminRequest)
    {
        var token = await _service.Login(loginAdminRequest);
        
        if (token == null)
            return Unauthorized();
        
        Response.Cookies.Append(
            "token",
            token,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = false, // пока локально HTTP
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(10)
            });
        
        
        return Ok(new LoginAdminResponse(token));
    } 
    
    
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterAdminRequest loginAdminRequest)
    {
        var adminDto = await _service.Register(loginAdminRequest);
        return Ok(adminDto);    
    } 
}