// Core/DTOs/ResponseDTOs/InputIngredientResponse.cs
namespace Core.DTOs.ResponseDTOs;

public class InputIngredientResponse
{
    public int Id { get; set; }
    public double Quantity { get; set; }
    public int InputId { get; set; }
    public int IngredientId { get; set; }
}
