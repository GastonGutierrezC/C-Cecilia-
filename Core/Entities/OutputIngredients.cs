using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class OutputIngredients: BaseEntity
{
    public double Quantity { get; set; }
    
    public required int IngredientId { get; set; }
    [ForeignKey(nameof(IngredientId))]
    public Ingredient? Ingredient { get; set; }
    
    public required int OutputId { get; set; }
    [ForeignKey(nameof(OutputId))]
    public Output? Output { get; set; }
}