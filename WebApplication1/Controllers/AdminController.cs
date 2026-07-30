using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("admin")]
public class AdminController(AdminService adminService) : ControllerBase
{
    private readonly AdminService _adminService = adminService;

    [HttpGet]
    public async Task<IActionResult> GetAdmins()
    {
        var admins = await _adminService.GetAll();
        return Ok(admins);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetAdmin(Guid id)
    {
        var admin = await _adminService.Get(id);
        return Ok(admin);
    }


    [HttpPost]
    public async Task<IActionResult> CreateAdmin(RegisterAdminDto adminDto)
    {
        var createdAdmin = await _adminService.Register(adminDto);
    
        return CreatedAtAction(
            nameof(GetAdmin),
            new { id = createdAdmin.Id },
            createdAdmin
        );
    }
    
}