using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class InputIngredients: BaseEntity
{
    public double Quantity { get; set; }
    public required int InputId { get; set; }
    [ForeignKey(nameof(InputId))]
    public Input? Input { get; set; }
    public required int IngredientId { get; set; }
    [ForeignKey(nameof(IngredientId))]
    public Ingredient? Ingredient { get; set; }
}