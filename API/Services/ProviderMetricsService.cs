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

public async Task<List<ProviderSeriesDto>> GetMonthlyProviderSummariesAsync(int month, string providerName)
{
    int year = DateTime.Today.Year;

    var providers = await _providerRepository.ListAllAsync();
    var selectedProviders = providers
        .Where(p => p.Name.Equals(providerName, StringComparison.OrdinalIgnoreCase))
        .ToList();

    if (!selectedProviders.Any())
        return new List<ProviderSeriesDto>();

    var outputs = await _outputRepository.ListAllAsync();
    var filteredOutputs = outputs
        .Where(o => o.OutputDate.Year == year && o.OutputDate.Month == month)
        .ToList();

    var outputIds = filteredOutputs.Select(o => o.Id).ToList();

    var outputProducts = await _outputProductsRepository.ListAllAsync();
    var filteredOutputProducts = outputProducts
        .Where(op => outputIds.Contains(op.OutputId))
        .ToList();

    var products = await _productRepository.ListAllAsync();

    var today = DateTime.Today;
    int daysInMonth = DateTime.DaysInMonth(year, month);
    int daysElapsed = (today.Year == year && today.Month == month) ? today.Day : daysInMonth;

    var result = new List<ProviderSeriesDto>();

    foreach (var provider in selectedProviders)
    {
        var providerProducts = products
            .Where(p => p.ProviderId == provider.Id)
            .Select(p => p.Id)
            .ToHashSet();

        var providerOutputProducts = filteredOutputProducts
            .Where(op => providerProducts.Contains(op.ProductId))
            .ToList();

        var dailySalesList = new List<DailySalesEntry>();

        var dailySalesGroups = filteredOutputs
            .GroupBy(o => o.OutputDate.Date)
            .ToList();

        foreach (var group in dailySalesGroups)
        {
            var opsOnDate = providerOutputProducts
                .Where(op => filteredOutputs.Any(o => o.Id == op.OutputId && o.OutputDate.Date == group.Key))
                .ToList();

            double dailyTotal = 0;
            foreach (var op in opsOnDate)
            {
                var product = products.FirstOrDefault(p => p.Id == op.ProductId);
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
        double requiredDailySales = daysRemaining > 0
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

        result.Add(summary);
    }

    return result;
}

    }
}
