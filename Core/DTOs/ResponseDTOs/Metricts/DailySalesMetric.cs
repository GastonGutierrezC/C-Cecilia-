namespace Core.DTOs.ResponseDTOs;

public class SalesMetricsDto2
{
    public DateOnly Date { get; set; }
    public double ProductSalesTotal { get; set; }
    public double IngredientSalesTotal { get; set; }
    public double CombinedSalesTotal => ProductSalesTotal + IngredientSalesTotal;
}
