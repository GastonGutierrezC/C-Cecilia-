namespace Core.Interfaces.Services;

using Core.DTOs.RequestDTOs;

public interface IInputProductService
{
    public Task<bool> RegisterInputProductoAsync(AutoInputProduct dto);
    public Task<bool> RegisterMultipleInputsAsync(List<AutoInputProduct> dtos);
}
