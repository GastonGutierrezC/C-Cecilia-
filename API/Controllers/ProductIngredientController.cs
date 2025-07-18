using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductIngredientController : ControllerBase
{
    private readonly IGenericRepository<ProductIngredients> _repository;
    private readonly IHomemadeProductService _service;
    private readonly IMapper _mapper;

    public ProductIngredientController(
        IGenericRepository<ProductIngredients> repository,
        IMapper mapper,
        IHomemadeProductService service)
    {
        _repository = repository;
        _mapper = mapper;
        _service = service;

    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductIngredientResponse>>> GetAll()
    {
        var items = await _repository.ListAllAsync();
        var result = items.Select(item => _mapper.Map<ProductIngredientResponse>(item));
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductIngredientResponse>> GetById(int id)
    {
        var item = await _repository.GetByIdAsync(id);
        if (item is null) return NotFound();
        return Ok(_mapper.Map<ProductIngredientResponse>(item));
    }

    [HttpPost]
    public async Task<ActionResult<bool>> Create(CreateProductIngredient dto)
    {
        await _repository.AddAsync(_mapper.Map<ProductIngredients>(dto));
        var saved = await _repository.SaveChangesAsync();
        return Ok(saved);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> Update(int id, UpdateProductIngredient dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return NotFound();

        _mapper.Map(dto, entity);
        await _repository.UpdateAsync(entity);
        var saved = await _repository.SaveChangesAsync();
        return Ok(saved);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return NotFound();

        await _repository.DeleteAsync(entity);
        var saved = await _repository.SaveChangesAsync();
        return Ok(saved);
    }
    
    [HttpGet("homemade")]
    public async Task<ActionResult<List<HomemadeProductGroupedResponse>>> GetHomemadeProducts()
    {
        var result = await _service.GetHomemadeProductsWithIngredientsAsync();
        return Ok(result);
    }

    [HttpPost("homemade")]
    public async Task<ActionResult<bool>> Create(CreateHomemadeProduct dto)
    {
        var result = await _service.RegisterHomemadeProductAsync(dto);
        if (!result)
            return BadRequest("Could not create the homemade product.");
        return Ok(true);
    }

    [HttpPut("homemade/{id}")]
    public async Task<ActionResult<bool>> UpdateHomemade(int id, CreateHomemadeProduct dto)
    {
        var result = await _service.UpdateHomemadeProductAsync(id, dto);
        if (!result)
            return BadRequest("Could not update the homemade product.");
        return Ok(true);
    }

    [HttpDelete("homemade/{id}")]
    public async Task<ActionResult<bool>> DeleteHomemade(int id)
    {
        try
        {
            var result = await _service.DeleteHomemadeProductAsync(id);
            if (!result)
                return BadRequest("No se pudo eliminar el producto.");

            return Ok(true);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
