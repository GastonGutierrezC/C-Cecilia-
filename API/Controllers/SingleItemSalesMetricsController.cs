using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SingleItemSalesMetricsController : ControllerBase
{
    private readonly ISingleItemSalesMetricsService _salesMetricsService;

    public SingleItemSalesMetricsController(ISingleItemSalesMetricsService salesMetricsService)
    {
        _salesMetricsService = salesMetricsService;
    }

    [HttpPost]
    public async Task<ActionResult<ProductSalesSeriesDto>> GetSingleItemSalesMetrics([FromBody] ItemSalesMetrictsRequestDto request)
    {

        var metrics = await _salesMetricsService.GetSalesByDateAndItemNameAsync(request.StartDate,request.EndDate,request.ItemId,request.IsProduct);
        return Ok(metrics);
        
    }
}
