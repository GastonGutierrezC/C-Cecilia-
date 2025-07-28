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
    public class ProviderMetricsController : ControllerBase
    {
        private readonly IProviderMetricsService _providerMetricsService;

        public ProviderMetricsController(IProviderMetricsService providerMetricsService)
        {
            _providerMetricsService = providerMetricsService;
        }

        [HttpPost]
        public async Task<ActionResult<List<ProviderSeriesDto>>> GetProviderMonthlySummary([FromBody] ProviderMetricsRequestDto request)
        {
            if (request.Month < 1 || request.Month > 12)
                return BadRequest("Invalid month. Must be between 1 and 12.");

            var summaries = await _providerMetricsService.GetMonthlyProviderSummariesAsync(request.Month,request.ProviderName);
            return Ok(summaries);
        }
    }
