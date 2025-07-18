using Core.DTOs.RequestDTOs;
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
    private readonly IGenericRepository<InputUser> _inputUserRepo;

    public InputService(
        IGenericRepository<Product> productRepo,
        IGenericRepository<ProductIngredients> productIngredientRepo,
        IGenericRepository<Ingredient> ingredientRepo,
        IGenericRepository<Input> inputRepo,
        IGenericRepository<InputProducts> inputProductRepo,
        IGenericRepository<InputUser> inputUserRepo)
    {
        _productRepo = productRepo;
        _productIngredientRepo = productIngredientRepo;
        _ingredientRepo = ingredientRepo;
        _inputRepo = inputRepo;
        _inputProductRepo = inputProductRepo;
        _inputUserRepo = inputUserRepo;
    }

    public async Task<bool> RegisterInputProductoAsync(AutoInputProduct dto, int userId)
    {
        var product = await _productRepo.GetByIdAsync(dto.ProductId);
        if (product == null)
        {
            throw new Exception("The product does not exist.");
        }

        var ingredientsAsociated = await _productIngredientRepo.ListAllAsync();
        var productsIngredient = ingredientsAsociated.Where(x => x.ProductId == dto.ProductId).ToList();
        bool homeMade = productsIngredient.Any();

        if (!homeMade)
        {
            var input = new Input { InputDate = DateTime.Now };
            await _inputRepo.AddAsync(input);
            await _inputRepo.SaveChangesAsync();
            product.Quantity += dto.Quantity;
            await _productRepo.UpdateAsync(product);
            var inputProduct = new InputProducts
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                InputId = input.Id
            };
            await _inputProductRepo.AddAsync(inputProduct);

            var inputUser = new InputUser
            {
                InputId = input.Id,
                UserId = userId
            };
            await _inputUserRepo.AddAsync(inputUser);
            return await _inputUserRepo.SaveChangesAsync();
        }
        else
        {
            var requiredQuantities = new Dictionary<int, double>();
            foreach (var recipe in productsIngredient)
            {
                double totalQuantity = recipe.Quantity * dto.Quantity;
                requiredQuantities[recipe.IngredientId] = totalQuantity;
            }

            foreach (var ingredientRequirement in requiredQuantities)
            {
                var ingredient = await _ingredientRepo.GetByIdAsync(ingredientRequirement.Key);
                if (ingredient == null)
                {
                    throw new Exception($"Ingredient with ID {ingredientRequirement.Key} not found.");
                }

                if (ingredient.Quantity < ingredientRequirement.Value)
                {
                    throw new Exception($"Not enough stock for ingredient ID {ingredientRequirement.Key}. Required: {ingredientRequirement.Value}, Available: {ingredient.Quantity}");
                }
            }

            var input = new Input { InputDate = DateTime.Now };
            await _inputRepo.AddAsync(input);
            await _inputRepo.SaveChangesAsync();

            foreach (var ingredientRequirement in requiredQuantities)
            {
                var ingredient = await _ingredientRepo.GetByIdAsync(ingredientRequirement.Key);
                if (ingredient == null)
                {
                    throw new Exception($"Ingredient with ID {ingredientRequirement.Key} not found.");
                }
                ingredient.Quantity -= ingredientRequirement.Value;
                await _ingredientRepo.UpdateAsync(ingredient);
            }

            product.Quantity += dto.Quantity;
            await _productRepo.UpdateAsync(product);

            var inputProduct = new InputProducts
            {
                ProductId = dto.ProductId,
                Quantity = dto.Quantity,
                InputId = input.Id
            };
            await _inputProductRepo.AddAsync(inputProduct);

            var inputUser = new InputUser
            {
                InputId = input.Id,
                UserId = userId
            };
            await _inputUserRepo.AddAsync(inputUser);

            return await _inputUserRepo.SaveChangesAsync();
        }
    }
}
