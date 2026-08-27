using WebApplication1.Dtos;
using WebApplication1.Models;
using WebApplication1.Repositories;

namespace WebApplication1.Services;

public class UserService(UsersRepository usersRepository)
{
    private readonly UsersRepository _usersRepository = usersRepository;


    public async Task<UserResponse> GetOrCreate(long telegramId, string username)
    {
        UserResponse? user = await Get(telegramId);
        if (user == null)
        {
            user = await Create(telegramId, username);
        }
        return user;
    }
    
    
    public async Task<List<UserResponse>> Get()
    {
        var list = await _usersRepository.Get();
        return list
            .Select(user => new UserResponse (
                user.Id, 
                user.Name ?? "Anonymous",
                user.Email ?? ""))
            .ToList();
    }
    
    public async Task<UserResponse?> Get(long id)
    {
        var user = await _usersRepository.Get(id);
        if (user != null)
        {
            return new UserResponse(user.Id, user.Name, user.Email);
        }
        return null;
    }


    public async Task<UserResponse> Create(long telegramId, string name)
    {
        var userEntity = new User
        {
            Id = Guid.NewGuid(),
            TelegramId = telegramId,
            Name = name,
        };
        await _usersRepository.Add(userEntity);
        
        return new UserResponse
        (
            userEntity.Id,
            userEntity.Name,
            userEntity.Email
        );
    }
    
    
    public Task<bool> Update(Guid id, UpdateUserRequest userRequest)
    {
        var userEntity = new User { 
            Name = userRequest.Name,
            TelegramId = userRequest.TelegramId
        };
        return _usersRepository.Update(id, userEntity);
    }
    
    public Task<bool> Delete(Guid id)
    {
        return _usersRepository.Delete(id);
    }
        
}