namespace Core.DTOs.RequestDTOs;

public class UpdateProductIngredient
{
    public double Quantity { get; set; }
    public int IngredientId { get; set; }
    public int ProductId { get; set; }
}
