using Core.DTOs.RequestDTOs;

namespace Core.Interfaces.Services;

public interface IOutputProductService
{
    Task<bool> RegisterOutputProductsAsync(List<CreateProductOutputRequest> outputRequests);
}