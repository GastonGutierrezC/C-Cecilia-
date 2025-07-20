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
public class ProviderController(
    IGenericRepository<Provider> repository,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Provider>>> GetProvider()
    {
        var res = await repository.ListAllAsync();
        return Ok(res.Select(prod => mapper.Map<ProviderResponse>(prod)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProviderResponse>> GetProviderById(int id)
    {
    var prod = await repository.GetByIdAsync(id);
    if (prod is null) return NotFound();

    return Ok(mapper.Map<ProviderResponse>(prod));
    }
 

    [HttpPost]
    public async Task<ActionResult<bool>> CreateProvider(CreateProvider prod)
    {
        await repository.AddAsync(mapper.Map<Provider>(prod));
        return Ok(await repository.SaveChangesAsync());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateProvider(int id, UpdateProvider dto)
    {
        var prod = await repository.GetByIdAsync(id);
        if (prod is null) return NotFound();

        mapper.Map(dto, prod);
        await repository.UpdateAsync(prod);
        return Ok(await repository.SaveChangesAsync());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteProvider(int id)
    {
        var prod = await repository.GetByIdAsync(id);
        if (prod is null) return NotFound();

        await repository.DeleteAsync(prod);
        return Ok(await repository.SaveChangesAsync());
    }
}