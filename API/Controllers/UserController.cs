using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using API.Services;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IGenericRepository<User> _repository;
    private readonly IMapper _mapper;
    private readonly TokenService _tokenService;
    private readonly AuthService _authService;

    public UserController(
        IGenericRepository<User> repository,
        IMapper mapper,
        TokenService tokenService,
        AuthService authService)
    {
        _repository = repository;
        _mapper = mapper;
        _tokenService = tokenService;
        _authService = authService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
    {
        var users = await _repository.ListAllAsync();
        return Ok(users.Select(_mapper.Map<UserResponse>));
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginUserDto loginDto)
    {
        var token = await _authService.LoginAsync(loginDto);

        if (token is null)
            return NotFound("Usuario no encontrado con ese username y email.");

        return Ok(token);
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<bool>> CreateUser(CreateUser dto)
    {
        await _repository.AddAsync(_mapper.Map<User>(dto));
        return Ok(await _repository.SaveChangesAsync());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateUser(int id, UpdateUser dto)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user is null) return NotFound();

        _mapper.Map(dto, user);
        await _repository.UpdateAsync(user);
        return Ok(await _repository.SaveChangesAsync());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteUser(int id)
    {
        var user = await _repository.GetByIdAsync(id);
        if (user is null) return NotFound();

        await _repository.DeleteAsync(user);
        return Ok(await _repository.SaveChangesAsync());
    }
}
