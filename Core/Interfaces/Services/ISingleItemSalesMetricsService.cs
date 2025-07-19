using Core.DTOs.ResponseDTOs;

namespace Core.Interfaces.Services;

public interface ISingleItemSalesMetricsService
{
    Task<List<SingleItemSalesMetricDto>> GetSalesByDateAndItemNameAsync(DateOnly startDate, DateOnly endDate, int itemId);

}
