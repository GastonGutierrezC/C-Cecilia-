// API/Controllers/OutputController.cs
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
public class OutputController: ControllerBase
{
    private readonly IGenericRepository<Output> _repository;
    private readonly IMapper _mapper;
    private readonly IOutputService _outputService;

    public OutputController(
        IGenericRepository<Output> repository,
        IMapper mapper,
        IOutputService outputService)
    {
        _repository = repository;
        _mapper = mapper;
        _outputService = outputService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Output>>> Get()
    {
        var list = await _repository.ListAllAsync();
        return Ok(list.Select(x => _mapper.Map<OutputResponse>(x)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<OutputResponse>> GetById(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return NotFound();
        return Ok(_mapper.Map<OutputResponse>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<bool>> Create(CreateOutput dto)
    {
        await _repository.AddAsync(_mapper.Map<Output>(dto));
        return Ok(await _repository.SaveChangesAsync());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> Update(int id, UpdateOutput dto)
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
    
    [HttpPost("register-combined")]
    public async Task<ActionResult<bool>> RegisterCombinedOutputs([FromBody] List<CreateOutputRequest> outputRequests)
    {
        try
        {
            var result = await _outputService.RegisterOutputsAsync(outputRequests);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

}
