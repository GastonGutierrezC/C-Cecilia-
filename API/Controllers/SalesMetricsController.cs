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
public class SalesMetricsController : ControllerBase
{
    private readonly ISalesMetricsService _salesMetricsService;

    public SalesMetricsController(ISalesMetricsService salesMetricsService)
    {
        _salesMetricsService = salesMetricsService;
    }

   [HttpPost]
    public async Task<ActionResult<List<SalesMetricsDto>>> GetSalesMetricsByDate([FromBody] SalesMetricsRequestDto request)
    {
        var metrics = await _salesMetricsService.GetSalesByDateRangeAsync(request.StartDate, request.EndDate);
        return Ok(metrics);
    }
}
