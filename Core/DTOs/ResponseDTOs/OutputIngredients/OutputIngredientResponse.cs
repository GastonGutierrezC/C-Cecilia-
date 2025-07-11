// Core/DTOs/ResponseDTOs/OutputIngredientResponse.cs
namespace Core.DTOs.ResponseDTOs;

public class OutputIngredientResponse
{
    public int Id { get; set; }
    public double Quantity { get; set; }
    public int IngredientId { get; set; }
    public int OutputId { get; set; }
}
