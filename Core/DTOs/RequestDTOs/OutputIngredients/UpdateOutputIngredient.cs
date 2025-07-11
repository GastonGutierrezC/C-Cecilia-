namespace Core.DTOs.RequestDTOs;

public class UpdateOutputIngredient
{
    public double Quantity { get; set; }
    public int IngredientId { get; set; }
    public int OutputId { get; set; }
}