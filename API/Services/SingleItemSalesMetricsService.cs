using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;

namespace API.Services;

public class SingleItemSalesMetricsService : ISingleItemSalesMetricsService
{
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IGenericRepository<Ingredient> _ingredientRepository;
    private readonly IGenericRepository<Output> _outputRepository;
    private readonly IGenericRepository<OutputProducts> _outputProductRepository;
    private readonly IGenericRepository<OutputIngredients> _outputIngredientRepository;

    public SingleItemSalesMetricsService(
        IGenericRepository<Product> productRepository,
        IGenericRepository<Ingredient> ingredientRepository,
        IGenericRepository<Output> outputRepository,
        IGenericRepository<OutputProducts> outputProductRepository,
        IGenericRepository<OutputIngredients> outputIngredientRepository)
    {
        _productRepository = productRepository;
        _ingredientRepository = ingredientRepository;
        _outputRepository = outputRepository;
        _outputProductRepository = outputProductRepository;
        _outputIngredientRepository = outputIngredientRepository;
    }

public async Task<SingleItemSalesMetricDto> GetSalesByDateAndItemNameAsync(DateOnly date, string itemName)
{
    var outputs = await _outputRepository.ListAllAsync();
    var filteredOutputs = outputs
        .Where(o => DateOnly.FromDateTime(o.OutputDate) == date)
        .ToList();

    if (!filteredOutputs.Any())
        return null;

    var outputIds = filteredOutputs.Select(o => o.Id).ToList();

    var allProducts = await _productRepository.ListAllAsync();
    var allIngredients = await _ingredientRepository.ListAllAsync();

    var product = allProducts.FirstOrDefault(p => 
        string.Equals(p.Name.Trim(), itemName.Trim(), StringComparison.OrdinalIgnoreCase));

    var ingredient = allIngredients.FirstOrDefault(i =>
        string.Equals(i.Name.Trim(), itemName.Trim(), StringComparison.OrdinalIgnoreCase));

    double totalSales = 0;
    int totalQuantity = 0;

    if (product != null)
    {
        var outputProducts = (await _outputProductRepository.ListAllAsync())
            .Where(op => outputIds.Contains(op.OutputId) && op.ProductId == product.Id)
            .ToList();

        totalQuantity = (int)outputProducts.Sum(op => op.Quantity);
        totalSales = outputProducts.Sum(op => op.Quantity * product.SellPrice);
    }
    else if (ingredient != null)
    {
        var outputIngredients = (await _outputIngredientRepository.ListAllAsync())
            .Where(oi => outputIds.Contains(oi.OutputId) && oi.IngredientId == ingredient.Id)
            .ToList();

        totalQuantity = (int)outputIngredients.Sum(oi => oi.Quantity);
        totalSales = outputIngredients.Sum(oi => oi.Quantity * ingredient.SellPrice);
    }
    else
    {
        return null;
    }

    return new SingleItemSalesMetricDto
    {
        Date = date,
        ItemName = itemName,
        QuantitySold = totalQuantity,
        TotalSales = totalSales
    };
}


}
