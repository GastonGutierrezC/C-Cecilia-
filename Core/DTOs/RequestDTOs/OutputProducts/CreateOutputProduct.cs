// Core/DTOs/RequestDTOs/CreateOutputProduct.cs
namespace Core.DTOs.RequestDTOs;

public class CreateOutputProduct
{
    public double Quantity { get; set; }
    public int OutputId { get; set; }
    public int ProductId { get; set; }
}
