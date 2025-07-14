namespace Core.Interfaces.Services;

using Core.DTOs.RequestDTOs;

public interface IInputService
{
    public Task<bool> RegisterInputProductoAsync(AutoInputProduct dto, int userId);
}
