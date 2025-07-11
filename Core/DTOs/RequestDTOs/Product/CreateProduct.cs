namespace Core.DTOs.RequestDTOs;

public class CreateProduct
{
    public required string Name { get; set; }
    public required double InPrice { get; set; }
    public required double SellPrice { get; set; }
    public required string? Image { get; set; }

}