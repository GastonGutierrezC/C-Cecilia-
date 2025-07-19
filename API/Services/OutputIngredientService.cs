using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;

namespace API.Services;

public class OutputIngredientService : IOutputIngredientService
{
    private readonly IGenericRepository<Output> _outputRepo;
    private readonly IGenericRepository<OutputIngredients> _outputIngredientRepo;
    private readonly IGenericRepository<Ingredient> _ingredientRepo;
    private readonly IMapper _mapper;

    public OutputIngredientService(
        IGenericRepository<Output> outputRepo,
        IGenericRepository<OutputIngredients> outputIngredientRepo,
        IGenericRepository<Ingredient> ingredientRepo,
        IMapper mapper)
    {
        _outputRepo = outputRepo;
        _outputIngredientRepo = outputIngredientRepo;
        _ingredientRepo = ingredientRepo;
        _mapper = mapper;
    }

    public async Task<bool> RegisterOutputIngredientsAsync(List<CreateIngredientOutputRequest> outputRequests)
    {
        if(outputRequests == null || !outputRequests.Any()) 
            throw new ArgumentException("Output requests cannot be null or empty.");

        var output = new Output
        {
            OutputDate = DateTime.UtcNow
        };

        await _outputRepo.AddAsync(output);
        await _outputRepo.SaveChangesAsync();

        foreach (var request in outputRequests)
        {
            var ingredient = await _ingredientRepo.GetByIdAsync(request.Id);
            if (ingredient == null)
                throw new Exception($"Product with ID {request.Id} does not exist.");

            if (ingredient.Quantity < request.Quantity)
                throw new Exception($"Insufficient quantity for product {ingredient.Name}. Available: {ingredient.Quantity}, Requested: {request.Quantity}");

            var outputProduct = new OutputIngredients
            {
                IngredientId = ingredient.Id,
                Quantity = request.Quantity,
                OutputId = output.Id
            };
            await _outputIngredientRepo.AddAsync(outputProduct);
            ingredient.Quantity -= request.Quantity;
            await _ingredientRepo.UpdateAsync(ingredient);
        }
        await _outputIngredientRepo.SaveChangesAsync();
        await _ingredientRepo.SaveChangesAsync();

        return true;

    }

}
