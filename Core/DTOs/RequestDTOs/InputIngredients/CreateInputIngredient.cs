namespace Core.DTOs.RequestDTOs;

public class CreateInputIngredient
{
    public double Quantity { get; set; }
    public int InputId { get; set; }
    public int IngredientId { get; set; }
}