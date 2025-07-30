using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class ProviderMetricsService : IProviderMetricsService
    {
        private readonly IGenericRepository<Output> _outputRepository;
        private readonly IGenericRepository<OutputProducts> _outputProductsRepository;
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<Provider> _providerRepository;

        public ProviderMetricsService(
            IGenericRepository<Output> outputRepository,
            IGenericRepository<OutputProducts> outputProductsRepository,
            IGenericRepository<Product> productRepository,
            IGenericRepository<Provider> providerRepository)
        {
            _outputRepository = outputRepository;
            _outputProductsRepository = outputProductsRepository;
            _productRepository = productRepository;
            _providerRepository = providerRepository;
        }

public async Task<List<ProviderSeriesDto>> GetMonthlyProviderSummariesAsync(int year, int month, int providerId)
{
    var today = DateTime.Today;


            var provider = await _providerRepository.GetByIdAsync(providerId);
    if (provider == null)
        return new List<ProviderSeriesDto>();

    
    var outputs = await _outputRepository.ListAllAsync();
    var filteredOutputs = outputs
        .Where(o => o.OutputDate.Year == year && o.OutputDate.Month == month)
        .ToList();

    if (!filteredOutputs.Any())
        return new List<ProviderSeriesDto>();

    var outputIds = filteredOutputs.Select(o => o.Id).ToList();

   
    var products = await _productRepository.ListAllAsync();
    var providerProducts = products
        .Where(p => p.ProviderId == providerId)
        .ToList();

    if (!providerProducts.Any())
        return new List<ProviderSeriesDto>();

    var providerProductIds = providerProducts.Select(p => p.Id).ToHashSet();

    var outputProducts = await _outputProductsRepository.ListAllAsync();
    var filteredOutputProducts = outputProducts
        .Where(op => outputIds.Contains(op.OutputId) && providerProductIds.Contains(op.ProductId))
        .ToList();

    if (!filteredOutputProducts.Any())
        return new List<ProviderSeriesDto>();

    int daysInMonth = DateTime.DaysInMonth(year, month);
    int daysElapsed = (today.Year == year && today.Month == month) ? today.Day : daysInMonth;

    var dailySalesList = new List<DailySalesEntry>();

    var dailySalesGroups = filteredOutputs
        .GroupBy(o => o.OutputDate.Date)
        .ToList();

    foreach (var group in dailySalesGroups)
    {
        var opsOnDate = filteredOutputProducts
            .Where(op => filteredOutputs.Any(o => o.Id == op.OutputId && o.OutputDate.Date == group.Key))
            .ToList();

        double dailyTotal = 0;
        foreach (var op in opsOnDate)
        {
            var product = providerProducts.FirstOrDefault(p => p.Id == op.ProductId);
            if (product != null)
            {
                dailyTotal += op.Quantity * product.SellPrice;
            }
        }

        if (dailyTotal > 0)
        {
            dailySalesList.Add(new DailySalesEntry
            {
                Name = group.Key.ToString("yyyy-MM-dd"),
                Value = dailyTotal
            });
        }
    }

    double accumulated = dailySalesList.Sum(d => d.Value);
    double percentageAchieved = provider.ObjetiveMount > 0
        ? (accumulated / provider.ObjetiveMount) * 100
        : 0;

    double closingTrend = daysElapsed > 0
        ? (accumulated / daysElapsed) * daysInMonth
        : 0;

    double trendPercentage = provider.ObjetiveMount > 0
        ? (closingTrend / provider.ObjetiveMount) * 100
        : 0;

    int daysRemaining = daysInMonth - daysElapsed;
    double requiredDailySales = (provider.ObjetiveMount > accumulated && daysRemaining > 0)
        ? (provider.ObjetiveMount - accumulated) / daysRemaining
        : 0;

    var summary = new ProviderSeriesDto
    {
        ProviderName = new ProviderNameDto { Name = provider.Name },
        TargetAmount = new TargetAmountDto { Name = "Monto objetivo", Value = provider.ObjetiveMount },
        AccumulatedToDate = new AccumulatedToDateDto { Name = "Acumulado hasta la fecha", Value = accumulated },
        PercentageAchieved = new PercentageAchievedDto { Name = "Porcentaje alcanzado", Value = percentageAchieved },
        ClosingTrend = new ClosingTrendDto { Name = "Tendencia de cierre", Value = closingTrend },
        TrendPercentage = new TrendPercentageDto { Name = "Porcentaje de tendencia", Value = trendPercentage },
        RequiredDailySales = new RequiredDailySalesDto { Name = "Ventas diarias requeridas", Value = requiredDailySales },
        Series = dailySalesList.OrderBy(d => d.Name).ToList()
    };

    return new List<ProviderSeriesDto> { summary };
}


    }
}
