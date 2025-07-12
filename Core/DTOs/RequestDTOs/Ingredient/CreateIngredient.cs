namespace Core.DTOs.RequestDTOs;

public class CreateIngredient
{
    public required string Name { get; set; }
    public double Quantity { get; set; }
    public required string IngredientUnit { get; set; }
    public double UnitPrice { get; set; }
    public required double SellPrice { get; set;}
}
