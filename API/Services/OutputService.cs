using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;

namespace API.Services;

public class OutputService : IOutputService
{
    private readonly IGenericRepository<Output> _outputRepo;
    private readonly IGenericRepository<OutputProducts> _outputProductRepo;
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IGenericRepository<OutputIngredients> _outputIngredientRepo;
    private readonly IGenericRepository<Ingredient> _ingredientRepo;

    public OutputService(
        IGenericRepository<Output> outputRepo,
        IGenericRepository<OutputProducts> outputProductRepo,
        IGenericRepository<Product> productRepo,
        IGenericRepository<OutputIngredients> outputIngredientRepo,
        IGenericRepository<Ingredient> ingredientRepo)
    {
        _outputRepo = outputRepo;
        _outputProductRepo = outputProductRepo;
        _productRepo = productRepo;
        _outputIngredientRepo = outputIngredientRepo;
        _ingredientRepo = ingredientRepo;
    }

    public async Task<bool> RegisterOutputsAsync(List<CreateOutputRequest> outputRequests)
    {
        if (outputRequests == null || !outputRequests.Any())
           throw new ArgumentException("Output requests cannot be null or empty.");

        var output = new Output
        {
            OutputDate = DateTime.UtcNow
        };

        await _outputRepo.AddAsync(output);
        await _outputRepo.SaveChangesAsync();
        foreach (var request in outputRequests)
        {
            if (request.IsProduct)
            {
                var product = await _productRepo.GetByIdAsync(request.Id);
                if (product == null)
                    throw new Exception($"Product with ID {request.Id} does not exist.");

                if (product.Quantity < request.Quantity)
                    throw new Exception($"Insufficient quantity for product {product.Name}. Available: {product.Quantity}, Requested: {request.Quantity}");

                var outputProduct = new OutputProducts
                {
                    ProductId = product.Id,
                    Quantity = request.Quantity,
                    OutputId = output.Id
                };

                await _outputProductRepo.AddAsync(outputProduct);
                product.Quantity -= request.Quantity;
                await _productRepo.UpdateAsync(product);

            }
            else
            {
               var ingredient = await _ingredientRepo.GetByIdAsync(request.Id);
                if (ingredient == null)
                    throw new Exception($"Ingredient with ID {request.Id} does not exist.");

                if (ingredient.Quantity < request.Quantity)
                  throw new Exception($"Insufficient quantity for ingredient {ingredient.Name}. Available: {ingredient.Quantity}, Requested: {request.Quantity}");

                var outputIngredient = new OutputIngredients
                {
                    IngredientId = ingredient.Id,
                    Quantity = request.Quantity,
                    OutputId = output.Id
                };

                await _outputIngredientRepo.AddAsync(outputIngredient);
                ingredient.Quantity -= request.Quantity;
                await _ingredientRepo.UpdateAsync(ingredient);

            }
        }

        await _outputProductRepo.SaveChangesAsync();
        await _productRepo.SaveChangesAsync();
        await _outputIngredientRepo.SaveChangesAsync();
        await _ingredientRepo.SaveChangesAsync();
        return true;
    }

}