namespace Core.DTOs.RequestDTOs;

public class CreateProductIngredient
{
    public double Quantity { get; set; }
    public int IngredientId { get; set; }
    public int ProductId { get; set; }
}