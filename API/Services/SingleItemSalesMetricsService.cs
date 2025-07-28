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

        public async Task<ProductSalesSeriesDto> GetSalesByDateAndItemNameAsync(
            DateOnly startDate, DateOnly endDate, int itemId, bool isProduct)
        {
            var outputs = await _outputRepository.ListAllAsync();
            var filteredOutputs = outputs
                .Where(o => DateOnly.FromDateTime(o.OutputDate) >= startDate &&
                            DateOnly.FromDateTime(o.OutputDate) <= endDate)
                .ToList();

            if (!filteredOutputs.Any())
                return null;

            var groupedByDate = filteredOutputs
                .GroupBy(o => DateOnly.FromDateTime(o.OutputDate))
                .OrderBy(g => g.Key);

            var dailyDataPoints = new List<DailySalesDataPointDto>();
            double totalSalesAccumulated = 0;
            string itemName = "";

            if (isProduct)
            {
                var product = await _productRepository.GetByIdAsync(itemId);
                if (product == null)
                    return null;

                itemName = product.Name;

                foreach (var group in groupedByDate)
                {
                    var groupOutputIds = group.Select(o => o.Id).ToList();

                    var outputProducts = (await _outputProductRepository.ListAllAsync())
                        .Where(op => groupOutputIds.Contains(op.OutputId) && op.ProductId == product.Id)
                        .ToList();

                    int totalQuantity = (int)outputProducts.Sum(op => op.Quantity);
                    double totalSales = outputProducts.Sum(op => op.Quantity * product.SellPrice);

                    if (totalQuantity == 0)
                        continue;

                    totalSalesAccumulated += totalSales;

                    dailyDataPoints.Add(new DailySalesDataPointDto
                    {
                        Name = group.Key.ToString("yyyy-MM-dd"),
                        Value = totalQuantity
                    });
                }
            }
            else
            {
                var ingredient = await _ingredientRepository.GetByIdAsync(itemId);
                if (ingredient == null)
                    return null;

                itemName = ingredient.Name;

                foreach (var group in groupedByDate)
                {
                    var groupOutputIds = group.Select(o => o.Id).ToList();

                    var outputIngredients = (await _outputIngredientRepository.ListAllAsync())
                        .Where(oi => groupOutputIds.Contains(oi.OutputId) && oi.IngredientId == ingredient.Id)
                        .ToList();

                    int totalQuantity = (int)outputIngredients.Sum(oi => oi.Quantity);
                    double totalSales = outputIngredients.Sum(oi => oi.Quantity * ingredient.SellPrice);

                    if (totalQuantity == 0)
                        continue;

                    totalSalesAccumulated += totalSales;

                    dailyDataPoints.Add(new DailySalesDataPointDto
                    {
                        Name = group.Key.ToString("yyyy-MM-dd"),
                        Value = totalQuantity
                    });
                }
            }

            return new ProductSalesSeriesDto
            {
                Name = itemName,
                Series = dailyDataPoints,
                Value = totalSalesAccumulated
            };
        }
    }
}
