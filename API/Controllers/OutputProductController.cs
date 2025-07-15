// API/Controllers/OutputProductController.cs
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
public class OutputProductController(
    IGenericRepository<OutputProducts> repository,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OutputProducts>>> Get()
    {
        var list = await repository.ListAllAsync();

        return Ok(list.Select(x => mapper.Map<OutputProductResponse>(x)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OutputProductResponse>> GetById(int id)
    {
        var entity = await repository.GetByIdAsync(id);
        if (entity is null) return NotFound();
        return Ok(mapper.Map<OutputProductResponse>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<bool>> Create(CreateOutputProduct dto)
    {
        await repository.AddAsync(mapper.Map<OutputProducts>(dto));
        return Ok(await repository.SaveChangesAsync());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> Update(int id, UpdateOutputProduct dto)
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
