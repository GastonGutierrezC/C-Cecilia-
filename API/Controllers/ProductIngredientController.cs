// API/Controllers/ProductIngredientController.cs
using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductIngredientController(
    IGenericRepository<ProductIngredients> repository,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductIngredients>>> GetAll()
    {
        var items = await repository.ListAllAsync();
        return Ok(items.Select(mapper.Map<ProductIngredientResponse>));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductIngredientResponse>> GetById(int id)
    {
        var item = await repository.GetByIdAsync(id);
        if (item is null) return NotFound();
        return Ok(mapper.Map<ProductIngredientResponse>(item));
    }

    [HttpPost]
    public async Task<ActionResult<bool>> Create(CreateProductIngredient dto)
    {
        repository.AddAsync(mapper.Map<ProductIngredients>(dto));
        return Ok(await repository.SaveChangesAsync());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> Update(int id, UpdateProductIngredient dto)
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
