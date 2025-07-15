// API/Controllers/InputProductController.cs
using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class InputProductController(
    IGenericRepository<InputProducts> repository,
    IMapper mapper, IInputService _inputService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InputProducts>>> GetAll()
    {
        var list = await repository.ListAllAsync();
        return Ok(list.Select(x => mapper.Map<InputProductResponse>(x)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InputProductResponse>> GetById(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return NotFound();
        return Ok(mapper.Map<InputProductResponse>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<bool>> Create(AutoInputProduct dto, int userId)
    {
        try
        {
            bool result = await _inputService.RegisterInputProductoAsync(dto, userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> Update(int id, UpdateInputProduct dto)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return NotFound();
        mapper.Map(dto, entity);
        await repository.UpdateAsync(entity);
        return Ok(await repository.SaveChangesAsync());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return NotFound();
        await repository.DeleteAsync(entity);
        return Ok(await repository.SaveChangesAsync());
    }
}
