using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services;

public class ProductIngredientService : IProductIngredientService
{
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IGenericRepository<Ingredient> _ingredientRepo;

    public ProductIngredientService(
        IGenericRepository<Product> productRepo,
        IGenericRepository<Ingredient> ingredientRepo)
    {
        _productRepo = productRepo;
        _ingredientRepo = ingredientRepo;
    }

    public async Task<List<ProductIngredientSimpleResponse>> GetAllProductsAndIngredientsAsync()
    {
        var products = await _productRepo.ListAllAsync();
        var ingredients = await _ingredientRepo.ListAllAsync();

        var productResponses = products.Select(p => new ProductIngredientSimpleResponse
        {
            Id = p.Id,
            Name = p.Name,
            IsProduct = true
        });

        var ingredientResponses = ingredients.Select(i => new ProductIngredientSimpleResponse
        {
            Id = i.Id,
            Name = i.Name,
            IsProduct = false
        });

        return productResponses.Concat(ingredientResponses).ToList();
    }
}
