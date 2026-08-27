using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Controllers;

[ApiController]
[Route("admin")]
public class AdminController(AdminService adminService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetAdmins()
    {
        var admins = await adminService.GetAll();
        return Ok(admins);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetAdmin(Guid id)
    {
        var admin = await adminService.Get(id);
        return Ok(admin);
    }


    [HttpPost]
    public async Task<IActionResult> CreateAdmin(RegisterAdminRequest adminRequest)
    {
        var createdAdmin = await adminService.Register(adminRequest);
    
        return CreatedAtAction(
            nameof(GetAdmin),
            new { id = createdAdmin.Id },
            createdAdmin
        );
    }
    
}