// API/Controllers/OutputIngredientController.cs
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
public class OutputIngredientController(
    IGenericRepository<OutputIngredients> repository,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OutputIngredients>>> Get()
    {
        var list = await repository.ListAllAsync();
        return Ok(list.Select(x => mapper.Map<OutputIngredientResponse>(x)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OutputIngredientResponse>> GetById(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return NotFound();
        return Ok(mapper.Map<OutputIngredientResponse>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<bool>> Create(CreateOutputIngredient dto)
    {
        repository.AddAsync(mapper.Map<OutputIngredients>(dto));
        return Ok(await repository.SaveChangesAsync());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> Update(int id, UpdateOutputIngredient dto)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return NotFound();

        mapper.Map(dto, entity);
        repository.UpdateAsync(entity);
        return Ok(await repository.SaveChangesAsync());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return NotFound();

        repository.DeleteAsync(entity);
        return Ok(await repository.SaveChangesAsync());
    }
}
