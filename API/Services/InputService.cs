using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;

namespace API.Services;


public class InputService : IInputService
{
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IGenericRepository<ProductIngredients> _productIngredientRepo;
    private readonly IGenericRepository<Ingredient> _ingredientRepo;
    private readonly IGenericRepository<Input> _inputRepo;
    private readonly IGenericRepository<InputProducts> _inputProductRepo;
    private readonly IGenericRepository<InputIngredients> _inputIngredientRepo;

    public InputService(
        IGenericRepository<Product> productRepo,
        IGenericRepository<ProductIngredients> productIngredientRepo,
        IGenericRepository<Ingredient> ingredientRepo,
        IGenericRepository<Input> inputRepo,
        IGenericRepository<InputProducts> inputProductRepo,
        IGenericRepository<InputIngredients> inputIngredientRepo)
    {
        _productRepo = productRepo;
        _productIngredientRepo = productIngredientRepo;
        _ingredientRepo = ingredientRepo;
        _inputRepo = inputRepo;
        _inputProductRepo = inputProductRepo;
        _inputIngredientRepo = inputIngredientRepo;
    }

    public async Task<bool> RegisterInputsAsync(List<CreateInputRequest> inputRequests)
    {
        if (inputRequests == null || !inputRequests.Any())
            throw new ArgumentException("Input requests cannot be null or empty.");

        var input = new Input
        {
            InputDate = DateTime.Now
        };

        await _inputRepo.AddAsync(input);
        await _inputRepo.SaveChangesAsync();
        var allProductIngredients = await _productIngredientRepo.ListAllAsync();
        foreach (var request in inputRequests)
        {
            if (request.IsProduct)
            {
                var product = await _productRepo.GetByIdAsync(request.Id);
                if (product == null)
                    throw new Exception($"Product with ID {request.Id} does not exist.");
                var ingredientsForProduct = allProductIngredients.Where(pi => pi.ProductId == request.Id).ToList();
                bool isHomemade = ingredientsForProduct.Any();

                if (isHomemade)
                {
                    var requiredQuantities = new Dictionary<int, double>();
                    foreach (var ingredientEntry in ingredientsForProduct)
                    {
                        double totalQuantity = ingredientEntry.Quantity * request.Quantity;
                        requiredQuantities[ingredientEntry.IngredientId] = totalQuantity;
                    }

                    foreach (var req in requiredQuantities)
                    {
                        var ingredient = await _ingredientRepo.GetByIdAsync(req.Key);
                        if (ingredient == null)
                            throw new Exception($"Ingredient with ID {req.Key} not found.");

                        if (ingredient.Quantity < req.Value)
                            throw new Exception($"Not enough stock for ingredient ID {req.Key}. Required: {req.Value}, Available: {ingredient.Quantity}");
                    }

                    foreach (var req in requiredQuantities)
                    {
                        var ingredient = await _ingredientRepo.GetByIdAsync(req.Key);
                        if (ingredient == null)
                            throw new Exception($"Ingredient with ID {req.Key} not found.");
                        ingredient.Quantity -= req.Value;
                        await _ingredientRepo.UpdateAsync(ingredient);
                    }
                }

                product.Quantity += request.Quantity;
                await _productRepo.UpdateAsync(product);

                var inputProduct = new InputProducts
                {
                    ProductId = product.Id,
                    Quantity = request.Quantity,
                    InputId = input.Id
                };
                await _inputProductRepo.AddAsync(inputProduct);
            }
            else
            {
                var ingredient = await _ingredientRepo.GetByIdAsync(request.Id);
                if (ingredient == null)
                    throw new Exception($"Ingredient with ID {request.Id} does not exist.");

                ingredient.Quantity += request.Quantity;
                await _ingredientRepo.UpdateAsync(ingredient);

                var inputIngredient = new InputIngredients
                {
                    IngredientId = ingredient.Id,
                    Quantity = request.Quantity,
                    InputId = input.Id
                };
                await _inputIngredientRepo.AddAsync(inputIngredient);
            }
        }
        await _inputProductRepo.SaveChangesAsync();
        await _productRepo.SaveChangesAsync();
        await _ingredientRepo.SaveChangesAsync();
        await _inputIngredientRepo.SaveChangesAsync();
        return true;
    }
}