namespace Core.Interfaces.Services;

using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;

public interface IHomemadeProductService
{
    Task<bool> RegisterHomemadeProductAsync(CreateHomemadeProduct dto);
    Task<bool> UpdateHomemadeProductAsync(int productId, CreateHomemadeProduct dto);
    Task<bool> DeleteHomemadeProductAsync(int productId);
    Task<List<HomemadeProductWithIngredientsResponse>> GetHomemadeProductsWithIngredientsAsync();

}
