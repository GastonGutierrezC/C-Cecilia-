namespace Core.DTOs.ResponseDTOs;

public class CreateInputRequest
{
    public int Id { get; set; }
    public bool IsProduct { get; set; }
    public double Quantity { get; set; }
}
