using Core.DTOs.ResponseDTOs;

namespace Core.Interfaces.Services;

public interface ISalesMetricsService
{
    Task<List<SalesMetricsDto>> GetSalesByDateRangeAsync(DateOnly startDate, DateOnly endDate);
}
