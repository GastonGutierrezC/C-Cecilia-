using Core.DTOs.ResponseDTOs;

namespace Core.Interfaces.Services;


public interface IExternalProductService
{
    Task<List<ProductResponse>> GetExternalProductsAsync();
}
