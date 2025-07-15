
using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class InputIngredientController(
    IGenericRepository<InputIngredients> repository,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<InputIngredients>>> Get()
    {
        var list = await repository.ListAllAsync();
        return Ok(list.Select(x => mapper.Map<InputIngredientResponse>(x)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InputIngredientResponse>> GetById(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return NotFound();
        return Ok(mapper.Map<InputIngredientResponse>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<bool>> Create(CreateInputIngredient dto)
    {
        await repository.AddAsync(mapper.Map<InputIngredients>(dto));
        return Ok(await repository.SaveChangesAsync());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> Update(int id, UpdateInputIngredient dto)
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
