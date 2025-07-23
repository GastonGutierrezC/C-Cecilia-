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
    public class MetricsController : ControllerBase
    {
        private readonly IProviderMetricsService _providerMetricsService;

        public MetricsController(IProviderMetricsService providerMetricsService)
        {
            _providerMetricsService = providerMetricsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProviderSeriesDto>>> GetProviderMonthlySummary(int month, string providerName)
        {
            if (month < 1 || month > 12)
                return BadRequest("Invalid month. Must be between 1 and 12.");

            var summaries = await _providerMetricsService.GetMonthlyProviderSummariesAsync(month, providerName);
            return Ok(summaries);
        }
    }
