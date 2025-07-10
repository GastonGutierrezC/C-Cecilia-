using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class OutputProducts: BaseEntity
{
    public double Quantity { get; set; }
    
    public int OutputId { get; set; }
    [ForeignKey(nameof(OutputId))]
    public Output? Output { get; set; }
    public int ProductId { get; set; }
    [ForeignKey(nameof(ProductId))]
    public Product? Product { get; set; }
    
}