using Core.DTOs.RequestDTOs;
using Core.Interfaces;
using API.Services;
using Core.Entities;

namespace API.Services;

public class AuthService
{
    private readonly IGenericRepository<User> _repository;
    private readonly TokenService _tokenService;

    public AuthService(IGenericRepository<User> repository, TokenService tokenService)
    {
        _repository = repository;
        _tokenService = tokenService;
    }

    public async Task<string?> LoginAsync(LoginUserDto loginDto)
    {
        var users = await _repository.ListAllAsync();

        var user = users.FirstOrDefault(u =>
            u.Username == loginDto.Username &&
            u.Email == loginDto.Email);

        if (user is null)
            return null;

        return _tokenService.CreateToken(user);
    }
}
