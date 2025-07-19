namespace Core.DTOs.ResponseDTOs;
public class SingleItemSalesMetricDto
{
    public DateOnly Date { get; set; }
    public string ItemName { get; set; }
    public int QuantitySold { get; set; }
    public double TotalSales { get; set; }
}
