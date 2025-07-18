
using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;

namespace API.Services;

public class HomemadeProductService : IHomemadeProductService
{
    private readonly IGenericRepository<Product> _productRepo;
    private readonly IGenericRepository<ProductIngredients> _productIngredientRepo;
    private readonly IGenericRepository<Ingredient> _ingredientRepo;
    private readonly IGenericRepository<InputProducts> _inputProductRepo;
    private readonly IGenericRepository<OutputProducts> _outputProductRepo;
    private readonly IMapper _mapper;

    public HomemadeProductService(
        IGenericRepository<Product> productRepo,
        IGenericRepository<Ingredient> ingredientRepo,
        IGenericRepository<ProductIngredients> productIngredientRepo, IMapper mapper, IGenericRepository<InputProducts> inputProductRepo,
        IGenericRepository<OutputProducts> outputProductRepo)
    {
        _productRepo = productRepo;
        _ingredientRepo = ingredientRepo;
        _productIngredientRepo = productIngredientRepo;
        _mapper = mapper;
        _inputProductRepo = inputProductRepo;
        _outputProductRepo = outputProductRepo;
    }

    public async Task<bool> RegisterHomemadeProductAsync(CreateHomemadeProduct dto)
    {
        foreach (var ing in dto.Ingredients)
        {
            var ingredient = await _ingredientRepo.GetByIdAsync(ing.IngredientId);
            if (ingredient == null)
            {
                throw new Exception($"Ingredient with ID {ing.IngredientId} does not exist.");
            }
        }

        var product = _mapper.Map<Product>(dto.Product);
        await _productRepo.AddAsync(product);
        await _productRepo.SaveChangesAsync();

        foreach (var ing in dto.Ingredients)
        {
            var productIngredient = _mapper.Map<ProductIngredients>(ing);
            productIngredient.ProductId = product.Id;
            await _productIngredientRepo.AddAsync(productIngredient);
        }
        return await _productIngredientRepo.SaveChangesAsync();
    }

    public async Task<bool> UpdateHomemadeProductAsync(int productId, CreateHomemadeProduct dto)
    {
        var product = await _productRepo.GetByIdAsync(productId);
        if (product == null)
            throw new Exception("Product not found.");

        foreach (var ingDto in dto.Ingredients)
        {
            var ingredient = await _ingredientRepo.GetByIdAsync(ingDto.IngredientId);
            if (ingredient == null)
            {
                throw new Exception($"Ingredient with ID {ingDto.IngredientId} does not exist.");
            }
        }

        _mapper.Map(dto.Product, product);
        await _productRepo.UpdateAsync(product);

        var currentProductIngredients = (await _productIngredientRepo.ListAllAsync())
                                        .Where(x => x.ProductId == productId)
                                        .ToList();

        var newIngredientIds = dto.Ingredients.Select(i => i.IngredientId).ToHashSet();
        var currentIngredientIds = currentProductIngredients.Select(pi => pi.IngredientId).ToHashSet();
        var toRemove = currentProductIngredients.Where(pi => !newIngredientIds.Contains(pi.IngredientId)).ToList();
        var toAddIngredientIds = newIngredientIds.Except(currentIngredientIds).ToList();
        var toUpdate = currentProductIngredients.Where(pi => newIngredientIds.Contains(pi.IngredientId)).ToList();

        foreach (var ingredientToRemove in toRemove)
        {
            await _productIngredientRepo.DeleteAsync(ingredientToRemove);
        }

        foreach (var ingredientIdToAdd in toAddIngredientIds)
        {
            var dtoIngredient = dto.Ingredients.First(i => i.IngredientId == ingredientIdToAdd);
            var newProductIngredient = _mapper.Map<ProductIngredients>(dtoIngredient);
            newProductIngredient.ProductId = productId;
            await _productIngredientRepo.AddAsync(newProductIngredient);
        }

        foreach (var ingredientToUpdate in toUpdate)
        {
            var dtoIngredient = dto.Ingredients.First(i => i.IngredientId == ingredientToUpdate.IngredientId);
            ingredientToUpdate.Quantity = dtoIngredient.Quantity;
            await _productIngredientRepo.UpdateAsync(ingredientToUpdate);
        }

        return await _productIngredientRepo.SaveChangesAsync();
    }

    public async Task<bool> DeleteHomemadeProductAsync(int productId)
    {
        var inputProducts = await _inputProductRepo.ListAllAsync();
        if (inputProducts.Any(x => x.ProductId == productId))
            throw new Exception("El producto está relacionado con una entrada y no puede eliminarse.");

        var outputProducts = await _outputProductRepo.ListAllAsync();
        if (outputProducts.Any(x => x.ProductId == productId))
            throw new Exception("El producto está relacionado con una salida y no puede eliminarse.");

        var productIngredients = await _productIngredientRepo.ListAllAsync();
        var ingredientsToDelete = productIngredients.Where(x => x.ProductId == productId).ToList();
        foreach (var deletes in ingredientsToDelete)
        {
            await _productIngredientRepo.DeleteAsync(deletes);
        }
        await _productIngredientRepo.SaveChangesAsync();

        var product = await _productRepo.GetByIdAsync(productId);
        if (product == null)
            throw new Exception("Producto no encontrado.");

        await _productRepo.DeleteAsync(product);
        return await _productRepo.SaveChangesAsync();
    }

    public async Task<List<HomemadeProductWithIngredientsResponse>> GetHomemadeProductsWithIngredientsAsync()
    {
        var products = await _productRepo.ListAllAsync();
        var productIngredients = await _productIngredientRepo.ListAllAsync();
        var ingredients = await _ingredientRepo.ListAllAsync();

        var homemadeProducts = products
            .Where(p => productIngredients.Any(pi => pi.ProductId == p.Id))
            .ToList();

        var responseList = new List<HomemadeProductWithIngredientsResponse>();

        foreach (var product in homemadeProducts)
        {
            var ingredientsForProduct = productIngredients
                .Where(pi => pi.ProductId == product.Id)
                .ToList();

            var ingredientDetails = new List<HomemadeIngredientResponse>();

            foreach (var pi in ingredientsForProduct)
            {
                var ingredient = ingredients.FirstOrDefault(i => i.Id == pi.IngredientId);
                if (ingredient != null)
                {
                    ingredientDetails.Add(new HomemadeIngredientResponse
                    {
                        Name = ingredient.Name,
                        Quantity = pi.Quantity,
                        IngredientUnit = ingredient.IngredientUnit,
                        UnitPrice = ingredient.UnitPrice,
                        SellPrice = ingredient.SellPrice
                    });
                }
            }

            responseList.Add(new HomemadeProductWithIngredientsResponse
            {
                Name = product.Name,
                InPrice = product.InPrice,
                SellPrice = product.SellPrice,
                Quantity = product.Quantity,
                Ingredients = ingredientDetails
            });
        }

        return responseList;
    }
}