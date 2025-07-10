using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController(
    IGenericRepository<Product> repository,
    IMapper mapper): ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
    {
        var res = await repository.ListAllAsync();
        return Ok(res.Select(prod => mapper.Map<ProductResponse>(prod)));
    }

    [HttpPost]
    public async Task<ActionResult<bool>> CreateProduct(CreateProduct prod)
    {
        repository.AddAsync(mapper.Map<Product>(prod));
        return Ok(await repository.SaveChangesAsync());
    }
    
}