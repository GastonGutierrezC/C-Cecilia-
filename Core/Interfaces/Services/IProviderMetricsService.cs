using Core.DTOs.ResponseDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IProviderMetricsService
    {
        Task<List<ProviderSeriesDto>> GetMonthlyProviderSummariesAsync(int year, int month, int providerId);
    }
}
