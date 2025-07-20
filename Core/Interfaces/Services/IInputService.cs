using Core.DTOs.ResponseDTOs;

namespace Core.Interfaces.Services;
public interface IInputService
{
    Task<bool> RegisterInputsAsync(List<CreateInputRequest> inputRequests);
}