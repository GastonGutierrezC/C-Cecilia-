namespace Core.DTOs.ResponseDTOs;

public class HomemadeProductGroupedResponse
{
    public required ProductResponse Product { get; set; }

    public required List<HomemadeIngredientResponse> Ingredients { get; set; }

}
