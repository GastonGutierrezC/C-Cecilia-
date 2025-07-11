namespace Core.DTOs.RequestDTOs;

public class CreateInputProduct
{
    public double Quantity { get; set; }
    public int InputId { get; set; }
    public int ProductId { get; set; }
}