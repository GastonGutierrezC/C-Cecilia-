namespace Core.Entities;

public class ProductResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required double InPrice { get; set; }
    public required double SellPrice { get; set; }
    public required string? Image { get; set; }
    public required double Quantity { get; set; }
}