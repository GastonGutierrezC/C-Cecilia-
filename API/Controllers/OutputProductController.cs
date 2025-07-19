// API/Controllers/OutputProductController.cs
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
public class OutputProductController : ControllerBase
{
    private readonly IGenericRepository<OutputProducts> _repository;
    private readonly IMapper _mapper;
    private readonly IOutputProductService _outputService;

    public OutputProductController(
        IGenericRepository<OutputProducts> repository,
        IMapper mapper,
        IOutputProductService outputService)
    {
        _repository = repository;
        _mapper = mapper;
        _outputService = outputService;
    }
    [HttpGet]
    public async Task<ActionResult<IEnumerable<OutputProducts>>> Get()
    {
        var list = await _repository.ListAllAsync();

        return Ok(list.Select(x => _mapper.Map<OutputProductResponse>(x)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OutputProductResponse>> GetById(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return NotFound();
        return Ok(_mapper.Map<OutputProductResponse>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<bool>> Create(CreateOutputProduct dto)
    {
        await _repository.AddAsync(_mapper.Map<OutputProducts>(dto));
        return Ok(await _repository.SaveChangesAsync());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> Update(int id, UpdateOutputProduct dto)
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

    [HttpPost("createOutputProducts")]
    public async Task<IActionResult> CreateOutputProducts([FromBody] List<CreateProductOutputRequest> outputRequests)
    {
        try
        {
            var result = await _outputService.RegisterOutputProductsAsync(outputRequests);
            if (!result)
                return BadRequest("No se pudo registrar la salida de productos.");

            return Ok(true);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
