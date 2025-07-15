
using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class IngredientController(
    IGenericRepository<Ingredient> repository,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Ingredient>>> GetIngredients()
    {
        var ingredients = await repository.ListAllAsync();
        return Ok(ingredients.Select(mapper.Map<IngredientResponse>));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<IngredientResponse>> GetIngredientById(int id)
    {
        var ingredient = await repository.GetByIdAsync(id);
        if (ingredient is null) return NotFound();

        return Ok(mapper.Map<IngredientResponse>(ingredient));
    }

    [HttpPost]
   public async Task<ActionResult<bool>> CreateIngredient(CreateIngredient dto)
   {

    await repository.AddAsync(mapper.Map<Ingredient>(dto));
    return Ok(await repository.SaveChangesAsync());
   }


    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateIngredient(int id, UpdateIngredient dto)
    {
        var ingredient = await repository.GetByIdAsync(id);
        if (ingredient is null) return NotFound();

        mapper.Map(dto, ingredient);
        await repository.UpdateAsync(ingredient);
        return Ok(await repository.SaveChangesAsync());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteIngredient(int id)
    {
        var ingredient = await repository.GetByIdAsync(id);
        if (ingredient is null) return NotFound();

        await repository.DeleteAsync(ingredient);
        return Ok(await repository.SaveChangesAsync());
    }
}
