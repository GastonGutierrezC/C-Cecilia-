namespace Core.DTOs.RequestDTOs;

public class CreateHomemadeProduct
{
    public required CreateProduct Product { get; set; }
    public required List<HomemadeIngredientDto> Ingredients { get; set; }
}
