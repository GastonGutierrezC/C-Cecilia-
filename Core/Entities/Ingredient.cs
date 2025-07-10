namespace Core.Entities;

public class Ingredient: BaseEntity
{
    public required string Name { get; set; }
    public double Quantity { get; set; }
    public required string IngredientUnit  { get; set; }
    public double UnitPrice { get; set; }
    
}