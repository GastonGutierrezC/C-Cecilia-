
namespace Core.Entities;

public class IngredientResponse
{
    public required int Id { get; set; }
    public required string Name { get; set; } = null!;
    public required double Quantity { get; set; }
    public required string IngredientUnit { get; set; } = null!;
    public required double UnitPrice { get; set; }
    public required double SellPrice { get; set;}
}
