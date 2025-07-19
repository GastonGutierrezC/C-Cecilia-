using AutoMapper;
using Core.DTOs.RequestDTOs;
using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;



namespace API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SingleItemSalesMetricsController : ControllerBase
{
    private readonly ISingleItemSalesMetricsService _salesMetricsService;

    public SingleItemSalesMetricsController(ISingleItemSalesMetricsService salesMetricsService)
    {
        _salesMetricsService = salesMetricsService;
    }

    [HttpGet("single-item")]
    public async Task<ActionResult<SingleItemSalesMetricDto>> GetSingleItemSalesMetrics([FromQuery] string itemName, [FromQuery] DateOnly date)
    {

        var metrics = await _salesMetricsService.GetSalesByDateAndItemNameAsync(date,itemName);
        return Ok(metrics);
        
    }
}
