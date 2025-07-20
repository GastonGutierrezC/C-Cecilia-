
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
public class InputController: ControllerBase
{
    private readonly IGenericRepository<Input> _repository;
    private readonly IMapper _mapper;
    private readonly IInputService _inputService;

    public InputController(
        IGenericRepository<Input> repository,
        IMapper mapper,
        IInputService inputService)
    {
        _repository = repository;
        _mapper = mapper;
        _inputService = inputService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Input>>> Get()
    {
        var list = await _repository.ListAllAsync();
        return Ok(list.Select(x => _mapper.Map<InputResponse>(x)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InputResponse>> GetById(int id)
    {
        var entity = await _repository.GetByIdAsync(id);
        if (entity is null) return NotFound();
        return Ok(_mapper.Map<InputResponse>(entity));
    }

    [HttpPost]
    public async Task<ActionResult<bool>> Create(CreateInput dto)
    {
        await _repository.AddAsync(_mapper.Map<Input>(dto));
        return Ok(await _repository.SaveChangesAsync());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> Update(int id, UpdateInput dto)
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

    [HttpPost("register")]
    public async Task<ActionResult<bool>> RegisterInputs([FromBody] List<CreateInputRequest> inputRequests)
    {
        try
        {
            var result = await _inputService.RegisterInputsAsync(inputRequests);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
