using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class UserService(UsersRepository usersRepository)
{
    private readonly UsersRepository _usersRepository = usersRepository;

    public async Task<List<UserDto>> Get()
    {
        var list = await _usersRepository.Get();
        return list
            .Select(user => new UserDto (
                user.Id, 
                user.Name, 
                user.Email))
            .ToList();
    }
    
    public async Task<UserDto?> Get(Guid id)
    {
        var user = await _usersRepository.Get(id);
        if (user != null)
        {
            return new UserDto(user.Id, user.Name, user.Email);
        }
        return null;
    }
    
    
    public async Task<UserDto> Create(CreateUserDto userDto)
    {
        var userEntity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Name = userDto.Name,
            Email = userDto.Email
        };
        await _usersRepository.Add(userEntity);
        
        return new UserDto
        (
            userEntity.Id,
            userEntity.Name,
            userEntity.Email
        );
    }
    
    
    public Task<bool> Update(Guid id, UpdateUserDto userDto)
    {
        var userEntity = new UserEntity { 
            Name = userDto.Name,
            Email = userDto.Email
        };
        return _usersRepository.Update(id, userEntity);
    }
    
    public Task<bool> Delete(Guid id)
    {
        return _usersRepository.Delete(id);
    }
        
}