namespace Core.DTOs.ResponseDTOs;

public class ProductIngredientResponse
{
    public int Id { get; set; }
    public double Quantity { get; set; }
    public int IngredientId { get; set; }
    public int ProductId { get; set; }
}