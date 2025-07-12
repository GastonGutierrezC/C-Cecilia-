namespace Core.Entities;

public class Product : BaseEntity
{
    public required string Name { get; set; }
    public double InPrice { get; set; }
    public double SellPrice { get; set;}
    public string? Image { get; set;}
    public double Quantity { get; set;}
    
}