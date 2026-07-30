using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class AdminService(AdminsRepository adminsRepository, JwtService jwtService)
{
    private readonly AdminsRepository _adminsRepository = adminsRepository;
    private readonly JwtService _jwtService = jwtService;
    
    public async Task<List<AdminDto>> GetAll()
    {
        var list = await _adminsRepository.Get();
        return list
            .Select(admin => new AdminDto(
                admin.Id,
                admin.Username))
            .ToList();
    }
    
    
    public async Task<AdminDto?> Get(Guid id)
    {
        var user = await _adminsRepository.Get(id);
        if (user != null)
        {
            return new AdminDto(user.Id, user.Username);
        }
        return null;
    }
    
    
    public async Task<AdminDto> Register(RegisterAdminDto adminDto)
    {
        // Create hash of password
        string hashPassword = BCrypt.Net.BCrypt.HashPassword(adminDto.Password);
        
        var adminEntity = new AdminEntity
        {
            Id = Guid.NewGuid(),
            Username = adminDto.Username,
            PasswordHash = hashPassword
        };
        
        await _adminsRepository.Add(adminEntity);
        
        return new AdminDto
        (
            adminEntity.Id,
            adminEntity.Username
        );
    }


    public async Task<string?> Login(LoginAdminDto adminDto)
    {
        var admin = await _adminsRepository.GetByUsername(adminDto.Username);
        if (admin == null)
            return null;

        var isValid = BCrypt.Net.BCrypt.Verify(adminDto.Password, admin.PasswordHash);

        if (!isValid)
            return null;

        return _jwtService.CreateToken(admin);
    }
}