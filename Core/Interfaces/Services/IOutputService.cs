using Core.DTOs.ResponseDTOs;

namespace Core.Interfaces.Services;

public interface IOutputService
{
    Task<bool> RegisterOutputsAsync(List<CreateOutputRequest> outputRequests);
}