

using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;

namespace API.Services;


public class ExternalProductService : IExternalProductService
{
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IGenericRepository<ProductIngredients> _productIngredientRepo;

    public ExternalProductService(
        IGenericRepository<Product> productRepo,
        IGenericRepository<ProductIngredients> productIngredientRepo)
    {
        _productRepo = productRepo;
        _productIngredientRepo = productIngredientRepo;
    }

    public async Task<List<ProductResponse>> GetExternalProductsAsync()
    {
        var products = await _productRepo.ListAllAsync();
        var productIngredients = await _productIngredientRepo.ListAllAsync();

        var homemadeIds = productIngredients.Select(pi => pi.ProductId).ToHashSet();

        var externalProducts = products
            .Where(p => !homemadeIds.Contains(p.Id))
            .ToList();

        return externalProducts.Select(p => new ProductResponse
        {
            Id = p.Id,
            Name = p.Name,
            InPrice = p.InPrice,
            SellPrice = p.SellPrice,
            Image = p.Image,
            Quantity = p.Quantity,
            ProviderId = p.ProviderId
        }).ToList();
    }
}
