using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class AdminService(AdminsRepository adminsRepository, JwtService jwtService)
{
    
    public async Task<List<AdminResponse>> GetAll()
    {
        var list = await adminsRepository.Get();
        return list
            .Select(admin => new AdminResponse(
                admin.Id,
                admin.Username))
            .ToList();
    }
    
    
    public async Task<AdminResponse?> Get(Guid id)
    {
        var user = await adminsRepository.Get(id);
        if (user != null)
        {
            return new AdminResponse(user.Id, user.Username);
        }
        return null;
    }
    
    
    public async Task<AdminResponse> Register(RegisterAdminRequest adminRequest)
    {
        // Create hash of password
        string hashPassword = BCrypt.Net.BCrypt.HashPassword(adminRequest.Password);
        
        var adminEntity = new Admin
        {
            Id = Guid.NewGuid(),
            Username = adminRequest.Username,
            PasswordHash = hashPassword
        };
        
        await adminsRepository.Add(adminEntity);
        
        return new AdminResponse
        (
            adminEntity.Id,
            adminEntity.Username
        );
    }


    public async Task<string?> Login(LoginAdminRequest adminRequest)
    {
        var admin = await adminsRepository.GetByUsername(adminRequest.Username);
        if (admin == null)
            return null;

        var isValid = BCrypt.Net.BCrypt.Verify(adminRequest.Password, admin.PasswordHash);

        if (!isValid)
            return null;

        return jwtService.CreateToken(admin);
    }
}