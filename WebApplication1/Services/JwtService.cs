using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WebApplication1.Models;

namespace WebApplication1.Services;

public class JwtService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string CreateToken(Admin admin)
    {
        
        var secretKey = _configuration["Jwt:Key"] 
                     ??
                     throw new InvalidOperationException("JWT Key is missing");
        
        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey)
        );
        
        var signingCredentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );


        var claims = new[]
        {
            new Claim(ClaimTypes.Name, admin.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };
        
        
        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(
                _configuration.GetValue<int>("Jwt:ExpireMinutes")
            ),
            signingCredentials: signingCredentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}