// Core/DTOs/ResponseDTOs/InputProductResponse.cs
namespace Core.DTOs.ResponseDTOs;

public class InputProductResponse
{
    public int Id { get; set; }
    public double Quantity { get; set; }
    public int InputId { get; set; }
    public int ProductId { get; set; }
}
