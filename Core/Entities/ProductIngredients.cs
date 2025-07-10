using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class ProductIngredients: BaseEntity
{
    public double Quantity { get; set; }
    
    public required int IngredientId { get; set; }
    [ForeignKey(nameof(IngredientId))]
    public Ingredient? Ingredient { get; set; }
    
    public required int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }
}