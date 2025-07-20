namespace Core.DTOs.ResponseDTOs;

public class CreateOutputRequest
{
    public int Id { get; set; }
    public bool IsProduct { get; set; }
    public double Quantity { get; set; }
}
