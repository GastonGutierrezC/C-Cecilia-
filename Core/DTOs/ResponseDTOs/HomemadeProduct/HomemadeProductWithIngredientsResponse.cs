namespace Core.DTOs.ResponseDTOs;

public class HomemadeProductWithIngredientsResponse
{
    public required string Name { get; set; }
    public required double InPrice { get; set; }
    public required double SellPrice { get; set; }
    public required double Quantity { get; set; }
    public required List<HomemadeIngredientResponse> Ingredients { get; set; }

}
