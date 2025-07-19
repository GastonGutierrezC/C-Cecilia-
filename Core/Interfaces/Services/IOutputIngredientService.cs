using Core.DTOs.RequestDTOs;

namespace Core.Interfaces.Services;

public interface IOutputIngredientService
{
    Task<bool> RegisterOutputIngredientsAsync(List<CreateIngredientOutputRequest> outputRequests);
}