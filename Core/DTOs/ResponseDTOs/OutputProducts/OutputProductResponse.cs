// Core/DTOs/ResponseDTOs/OutputProductResponse.cs
namespace Core.DTOs.ResponseDTOs;

public class OutputProductResponse
{
    public int Id { get; set; }
    public double Quantity { get; set; }
    public int OutputId { get; set; }
    public int ProductId { get; set; }
}
