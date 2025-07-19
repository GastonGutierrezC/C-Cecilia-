using Core.DTOs.ResponseDTOs;

namespace Core.Interfaces.Services;

public interface ISingleItemSalesMetricsService
{
    Task<SingleItemSalesMetricDto> GetSalesByDateAndItemNameAsync(DateOnly date, string itemName);
}
