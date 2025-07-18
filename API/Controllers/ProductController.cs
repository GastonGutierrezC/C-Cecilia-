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
public class ProductController(
    IGenericRepository<Product> repository,
    IMapper mapper) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var res = await repository.ListAllAsync();
        return Ok(res.Select(prod => mapper.Map<ProductResponse>(prod)));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductResponse>> GetProductById(int id)
    {
    var prod = await repository.GetByIdAsync(id);
    if (prod is null) return NotFound();

    return Ok(mapper.Map<ProductResponse>(prod));
    }
 

    [HttpPost]
    public async Task<ActionResult<bool>> CreateProduct(CreateProduct prod)
    {
        await repository.AddAsync(mapper.Map<Product>(prod));
        return Ok(await repository.SaveChangesAsync());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<bool>> UpdateProduct(int id, UpdateProduct dto)
    {
        var prod = await repository.GetByIdAsync(id);
        if (prod is null) return NotFound();

        mapper.Map(dto, prod);
        await repository.UpdateAsync(prod);
        return Ok(await repository.SaveChangesAsync());
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> DeleteProduct(int id)
    {
        var prod = await repository.GetByIdAsync(id);
        if (prod is null) return NotFound();

        await repository.DeleteAsync(prod);
        return Ok(await repository.SaveChangesAsync());
    }




}