using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class InputProducts: BaseEntity
{
    public double Quantity { get; set; }
    
    public int InputId { get; set; }
    [ForeignKey(nameof(InputId))]
    public Input? Input { get; set; }
    public int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }
}