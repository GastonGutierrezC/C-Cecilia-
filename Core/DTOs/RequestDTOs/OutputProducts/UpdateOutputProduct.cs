// Core/DTOs/RequestDTOs/UpdateOutputProduct.cs
namespace Core.DTOs.RequestDTOs;

public class UpdateOutputProduct
{
    public double Quantity { get; set; }
    public int OutputId { get; set; }
    public int ProductId { get; set; }
}
