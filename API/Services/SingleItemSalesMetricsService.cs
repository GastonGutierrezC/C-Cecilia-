using Core.DTOs.ResponseDTOs;
using Core.Entities;
using Core.Interfaces;
using Core.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Services
{
    public class SingleItemSalesMetricsService : ISingleItemSalesMetricsService
    {
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<Ingredient> _ingredientRepository;
        private readonly IGenericRepository<Output> _outputRepository;
        private readonly IGenericRepository<OutputProducts> _outputProductRepository;
        private readonly IGenericRepository<OutputIngredients> _outputIngredientRepository;

        public SingleItemSalesMetricsService(
            IGenericRepository<Product> productRepository,
            IGenericRepository<Ingredient> ingredientRepository,
            IGenericRepository<Output> outputRepository,
            IGenericRepository<OutputProducts> outputProductRepository,
            IGenericRepository<OutputIngredients> outputIngredientRepository)
        {
            _productRepository = productRepository;
            _ingredientRepository = ingredientRepository;
            _outputRepository = outputRepository;
            _outputProductRepository = outputProductRepository;
            _outputIngredientRepository = outputIngredientRepository;
        }

        public async Task<List<ProductSalesSeriesDto>> GetSalesByDateAndItemNameAsync(DateOnly startDate, DateOnly endDate, int itemId)
        {
            var outputs = await _outputRepository.ListAllAsync();
            var filteredOutputs = outputs
                .Where(o => DateOnly.FromDateTime(o.OutputDate) >= startDate &&
                            DateOnly.FromDateTime(o.OutputDate) <= endDate)
                .ToList();

            if (!filteredOutputs.Any())
                return new List<ProductSalesSeriesDto>();

            var outputIds = filteredOutputs.Select(o => o.Id).ToList();

            var product = await _productRepository.GetByIdAsync(itemId);
            var ingredient = await _ingredientRepository.GetByIdAsync(itemId);

            var groupedByDate = filteredOutputs
                .GroupBy(o => DateOnly.FromDateTime(o.OutputDate))
                .OrderBy(g => g.Key);

            string itemName = product != null ? product.Name : ingredient?.Name ?? "";

            var dailyDataPoints = new List<DailySalesDataPointDto>();
            double totalSalesAccumulated = 0;

            foreach (var group in groupedByDate)
            {
                var groupOutputIds = group.Select(o => o.Id).ToList();
                int totalQuantity = 0;
                double totalSales = 0;

                if (product != null)
                {
                    var outputProducts = (await _outputProductRepository.ListAllAsync())
                        .Where(op => groupOutputIds.Contains(op.OutputId) && op.ProductId == product.Id)
                        .ToList();

                    totalQuantity = (int)outputProducts.Sum(op => op.Quantity);
                    totalSales = outputProducts.Sum(op => op.Quantity * product.SellPrice);
                }
                else if (ingredient != null)
                {
                    var outputIngredients = (await _outputIngredientRepository.ListAllAsync())
                        .Where(oi => groupOutputIds.Contains(oi.OutputId) && oi.IngredientId == ingredient.Id)
                        .ToList();

                    totalQuantity = (int)outputIngredients.Sum(oi => oi.Quantity);
                    totalSales = outputIngredients.Sum(oi => oi.Quantity * ingredient.SellPrice);
                }
                else
                {
                    continue;
                }

                totalSalesAccumulated += totalSales;

                dailyDataPoints.Add(new DailySalesDataPointDto
                {
                    Name = group.Key.ToString("yyyy-MM-dd"),
                    Value = totalQuantity
                });
            }

            return new List<ProductSalesSeriesDto>
            {
                new ProductSalesSeriesDto
                {
                    Name = itemName,
                    Series = dailyDataPoints,
                    Value = totalSalesAccumulated
                }
            };
        }
    }
}
