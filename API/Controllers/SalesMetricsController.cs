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
public class SalesMetricsController : ControllerBase
{
    private readonly ISalesMetricsService _salesMetricsService;

    public SalesMetricsController(ISalesMetricsService salesMetricsService)
    {
        _salesMetricsService = salesMetricsService;
    }

[HttpGet]
public async Task<ActionResult<List<SalesMetricsDto>>> GetSalesMetricsByDate([FromQuery] DateOnly date)
{
    var metrics = await _salesMetricsService.GetSalesByDateRangeAsync(date);
    return Ok(metrics);
}
}
