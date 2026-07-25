using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class UserService(UsersRepository usersRepository)
{
    private readonly UsersRepository _usersRepository = usersRepository;


    public async Task<UserDto> GetOrCreate(long telegramId, string username)
    {
        UserDto? user = await Get(telegramId);
        if (user == null)
        {
            user = await Create(telegramId, username);
        }
        return user;
    }
    
    
    public async Task<List<UserDto>> Get()
    {
        var list = await _usersRepository.Get();
        return list
            .Select(user => new UserDto (
                user.Id, 
                user.Name))
            .ToList();
    }
    
    public async Task<UserDto?> Get(long id)
    {
        var user = await _usersRepository.Get(id);
        if (user != null)
        {
            return new UserDto(user.Id, user.Name);
        }
        return null;
    }


    public async Task<UserDto> Create(long telegramId, string name)
    {
        var userEntity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Name = name,
        };
        await _usersRepository.Add(userEntity);
        
        return new UserDto
        (
            userEntity.Id,
            userEntity.Name
        );
    }
    
    
    public Task<bool> Update(Guid id, UpdateUserDto userDto)
    {
        var userEntity = new UserEntity { 
            Name = userDto.Name,
            TelegramId = userDto.TelegramId
        };
        return _usersRepository.Update(id, userEntity);
    }
    
    public Task<bool> Delete(Guid id)
    {
        return _usersRepository.Delete(id);
    }
        
}