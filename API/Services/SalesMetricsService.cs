using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces.Services;

namespace API.Services;

public class SalesMetricsService : ISalesMetricsService
{
    private readonly IGenericRepository<Output> _outputRepo;
    private readonly IGenericRepository<OutputProducts> _outputProductsRepo;
    private readonly IGenericRepository<OutputIngredients> _outputIngredientsRepo;
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Ingredient> _ingredientRepository;

    public SalesMetricsService(
        IGenericRepository<Output> outputRepo,
        IGenericRepository<OutputProducts> outputProductsRepo,
        IGenericRepository<OutputIngredients> outputIngredientsRepo,
        IGenericRepository<Product> productRepository,
        IGenericRepository<Ingredient> ingredientRepository)
    {
        _outputRepo = outputRepo;
        _outputProductsRepo = outputProductsRepo;
        _outputIngredientsRepo = outputIngredientsRepo;
        _productRepository = productRepository;
        _ingredientRepository = ingredientRepository;
    }

    public async Task<List<SalesMetricsDto>> GetSalesByDateRangeAsync(DateOnly date)
    {
        var outputs = await _outputRepo.ListAllAsync();

        var filteredOutputs = outputs
            .Where(o => DateOnly.FromDateTime(o.OutputDate) == date)
            .ToList();

        var outputIds = filteredOutputs.Select(o => o.Id).ToList();

        var allProducts = await _productRepository.ListAllAsync();
        var allIngredients = await _ingredientRepository.ListAllAsync();

        var outputProducts = (await _outputProductsRepo.ListAllAsync())
            .Where(op => outputIds.Contains(op.OutputId))
            .ToList();

        var outputIngredients = (await _outputIngredientsRepo.ListAllAsync())
            .Where(oi => outputIds.Contains(oi.OutputId))
            .ToList();

        double productTotal = outputProducts.Sum(op =>
        {
            var product = allProducts.FirstOrDefault(p => p.Id == op.ProductId);
            return product != null ? op.Quantity * product.SellPrice : 0;
        });

        double ingredientTotal = outputIngredients.Sum(oi =>
        {
            var ingredient = allIngredients.FirstOrDefault(i => i.Id == oi.IngredientId);
            return ingredient != null ? oi.Quantity * ingredient.SellPrice : 0;
        });

        return new List<SalesMetricsDto>
        {
            new SalesMetricsDto
            {
                Date = date,
                ProductSalesTotal = productTotal,
                IngredientSalesTotal = ingredientTotal
            }
        };
    }
}
