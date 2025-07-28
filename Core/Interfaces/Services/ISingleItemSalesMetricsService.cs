using Core.DTOs.ResponseDTOs;

namespace Core.Interfaces.Services;

public interface ISingleItemSalesMetricsService
{
    Task<ProductSalesSeriesDto> GetSalesByDateAndItemNameAsync(DateOnly startDate, DateOnly endDate, int itemId,bool isProduct);

}
