// API/Controllers/OutputIngredientController.cs
using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace API.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class OutputIngredientController : ControllerBase
{
    private readonly IGenericRepository<OutputIngredients> _repository;
    private readonly IMapper _mapper;
    private readonly IOutputIngredientService _outputIngredientService;

    public OutputIngredientController(
        IGenericRepository<OutputIngredients> repository,
        IMapper mapper,
        IOutputIngredientService outputIngredientService)
    {
        _repository = repository;
        _mapper = mapper;
        _outputIngredientService = outputIngredientService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<OutputIngredients>>> Get()
    {
        var list = await _repository.ListAllAsync();
        return Ok(list.Select(x => _mapper.Map<OutputIngredientResponse>(x)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OutputIngredientResponse>> GetById(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return NotFound();
        return Ok(_mapper.Map<OutputIngredientResponse>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<bool>> Create(CreateOutputIngredient dto)
    {
        await _repository.AddAsync(_mapper.Map<OutputIngredients>(dto));
        return Ok(await _repository.SaveChangesAsync());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> Update(int id, UpdateOutputIngredient dto)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return NotFound();

        _mapper.Map(dto, entity);
        await _repository.UpdateAsync(entity);
        return Ok(await _repository.SaveChangesAsync());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return NotFound();

        await _repository.DeleteAsync(entity);
        return Ok(await _repository.SaveChangesAsync());
    }
    
    [HttpPost("createOutputIngredients")]
    public async Task<IActionResult> CreateOutputIngredients([FromBody] List<CreateIngredientOutputRequest> outputRequests)
    {
        try
        {
            var result = await _outputIngredientService.RegisterOutputIngredientsAsync(outputRequests);
            if (!result)
                return BadRequest("No se pudo registrar la salida de ingredientes.");

            return Ok(true);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
