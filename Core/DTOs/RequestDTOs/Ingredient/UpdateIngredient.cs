namespace Core.DTOs.RequestDTOs;

public class UpdateIngredient
{
    public required string Name { get; set; }
    public double Quantity { get; set; }
    public required string IngredientUnit { get; set; }
    public double UnitPrice { get; set; }
}
