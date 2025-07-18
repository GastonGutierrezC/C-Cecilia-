namespace Core.DTOs.ResponseDTOs;

public class HomemadeIngredientResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required double Quantity { get; set; }
    public required string IngredientUnit { get; set; }
    public required double UnitPrice { get; set; }
    public required double SellPrice { get; set; }
}
