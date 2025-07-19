namespace Core.DTOs.ResponseDTOs;

public class SalesMetricsDto
{
    public DateOnly Date { get; set; }
    public double ProductSalesTotal { get; set; }
    public double IngredientSalesTotal { get; set; }
    public double CombinedSalesTotal => ProductSalesTotal + IngredientSalesTotal;
}
