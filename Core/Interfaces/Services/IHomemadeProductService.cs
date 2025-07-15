namespace Core.Interfaces.Services;

using Core.DTOs.RequestDTOs;

public interface IHomemadeProductService
{
    Task<bool> RegisterHomemadeProductAsync(CreateHomemadeProduct dto);
    Task<bool> UpdateHomemadeProductAsync(int productId, CreateHomemadeProduct dto);
}
