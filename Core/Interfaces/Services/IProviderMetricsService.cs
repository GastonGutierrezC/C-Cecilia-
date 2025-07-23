using Core.DTOs.ResponseDTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.Services
{
    public interface IProviderMetricsService
    {
        Task<List<ProviderSalesSummaryDto>> GetMonthlyProviderSummariesAsync(int month, string providerName);
    }
}
