using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController(
    IGenericRepository<User> repository,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        var users = await repository.ListAllAsync();
        return Ok(users.Select(mapper.Map<UserResponse>));
    }

    [HttpGet("login")]
    public async Task<ActionResult<UserResponse>> GetUserByUsernameAndEmail(
        [FromQuery] string username,
        [FromQuery] string email)
    {
        var users = await repository.ListAllAsync();
        var user = users.FirstOrDefault(u => u.Username == username && u.Email == email);

        if (user is null)
            return NotFound("Usuario no encontrado con ese username y email.");

        return Ok(mapper.Map<UserResponse>(user));
    }

    [HttpPost]
    public async Task<ActionResult<bool>> CreateUser(CreateUser dto)
    {

        await repository.AddAsync(mapper.Map<User>(dto));
        return Ok(await repository.SaveChangesAsync());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateUser(int id, UpdateUser dto)
    {
        var user = await repository.GetByIdAsync(id);
        if (user is null) return NotFound();

        mapper.Map(dto, user);
        await repository.UpdateAsync(user);
        return Ok(await repository.SaveChangesAsync());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteUser(int id)
    {
        var user = await repository.GetByIdAsync(id);
        if (user is null) return NotFound();

        await repository.DeleteAsync(user);
        return Ok(await repository.SaveChangesAsync());
    }
}
