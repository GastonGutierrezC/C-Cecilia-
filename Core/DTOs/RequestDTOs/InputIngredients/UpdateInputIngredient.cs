namespace Core.DTOs.RequestDTOs;

public class UpdateInputIngredient
{
    public double Quantity { get; set; }
    public int InputId { get; set; }
    public int IngredientId { get; set; }
}